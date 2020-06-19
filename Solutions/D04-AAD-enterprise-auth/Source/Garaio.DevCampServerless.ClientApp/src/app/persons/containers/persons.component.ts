import { Component, OnInit, OnDestroy, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MDBModalRef, MDBModalService } from 'angular-bootstrap-md';
import { environment } from 'src/environments/environment';
import { Subscription, Subject } from 'rxjs';
import { take, switchMap, tap } from 'rxjs/operators';

import { Project } from '../../shared/models/project.model';
import { Person } from '../../shared/models/person.model';
import { PersonsModalComponent } from '../components/modal/persons-modal.component';
import { Technology } from 'src/app/shared/models/technology.model';

@Component({
  selector: 'app-persons',
  templateUrl: './persons.component.html',
  styleUrls: ['./persons.component.scss']
})
export class PersonsComponent implements OnInit, OnDestroy {
  isLoading$: Subject<boolean> = new Subject<boolean>();

  projects: Project[];
  technologies: Technology[];
  persons: Person[] | null;
  modalRef: MDBModalRef;

  gridDataSub: Subscription;
  loadGridData: EventEmitter<any> = new EventEmitter();

  modalConfig = {
        class: 'modal-dialog modal-dialog-scrollable'
  };

  constructor(private modalService: MDBModalService, private http: HttpClient) {
    this.gridDataSub = this.loadGridData.pipe(
      tap(_ => this.isLoading$.next(true)),
      switchMap(_ => this.http.get<Person[]>(`${environment.apiBaseUrl}/persons?code=${environment.apiAuthCode}`)),
      tap(_ => this.isLoading$.next(false))
    ).subscribe((persons: Person[]) => {
      this.persons = persons;
    });
  }

  ngOnInit() {
    this.loadGridData.emit(null);

    this.http.get<Project[]>(`${environment.apiBaseUrl}/projects?code=${environment.apiAuthCode}`)
    .pipe(take(1))
    .subscribe(p => this.projects = p);

    this.http.get<Technology[]>(`${environment.apiBaseUrl}/technologies?code=${environment.apiAuthCode}`)
    .pipe(take(1))
    .subscribe(t => this.technologies = t);
  }

  ngOnDestroy() {
    if (this.gridDataSub) {
      this.gridDataSub.unsubscribe();
    }
  }

  onAddPerson() {
    this.modalRef = this.modalService.show(PersonsModalComponent, this.modalConfig);

    this.modalRef.content.heading = 'Add new person';
    this.modalRef.content.technologies = this.technologies;
    this.modalRef.content.projects = this.projects;

    this.modalRef.content.personData.pipe(
      take(1),
      switchMap((personData: Person) => {
        const url = `${environment.apiBaseUrl}/persons?code=${environment.apiAuthCode}`;
        return this.http.post<Person>(url, personData);
      })).subscribe((_: any) => this.loadGridData.emit(null));
  }

  openEditPersonModal(person: Person) {
    this.modalRef = this.modalService.show(PersonsModalComponent, this.modalConfig);

    this.modalRef.content.heading = 'Edit person';
    this.modalRef.content.technologies = this.technologies;
    this.modalRef.content.projects = this.projects;
    this.modalRef.content.person = Object.assign({}, person);

    this.modalRef.content.personData.pipe(
      take(1),
      switchMap((personData: Person) => {
        const url = `${environment.apiBaseUrl}/persons/${person.rowKey}?code=${environment.apiAuthCode}`;
        return this.http.put<Person>(url, personData);
      })).subscribe((_: any) => this.loadGridData.emit(null));
  }

  onPersonEdit(person: Person) {
    const url = `${environment.apiBaseUrl}/persons/${person.rowKey}?code=${environment.apiAuthCode}`;
    this.http.get<Person>(url).pipe(take(1)).subscribe(p => this.openEditPersonModal(p));
  }

  onPersonDelete(person: Person) {
    const url = `${environment.apiBaseUrl}/persons/${person.rowKey}?code=${environment.apiAuthCode}`;
    this.http.delete(url).pipe(take(1)).subscribe(_ => this.loadGridData.emit(null));
  }
}
