import { Component, OnInit, OnDestroy, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MDBModalRef, MDBModalService } from 'angular-bootstrap-md';
import { environment } from 'src/environments/environment';
import { Subscription, Subject } from 'rxjs';
import { take, switchMap, tap } from 'rxjs/operators';

import { TechnologiesModalComponent } from '../components/modal/technologies-modal.component';
import { Technology } from 'src/app/shared/models/technology.model';

@Component({
  selector: 'app-technologies',
  templateUrl: './technologies.component.html',
  styleUrls: ['./technologies.component.scss']
})
export class TechnologiesComponent implements OnInit, OnDestroy {
  isLoading$: Subject<boolean> = new Subject<boolean>();

  technologies: Technology[] | null;
  modalRef: MDBModalRef;

  gridDataSub: Subscription;
  loadGridData: EventEmitter<any> = new EventEmitter();

  modalConfig = {
        class: 'modal-dialog modal-dialog-scrollable'
  };

  constructor(private modalService: MDBModalService, private http: HttpClient) {
    this.gridDataSub = this.loadGridData.pipe(
      tap(_ => this.isLoading$.next(true)),
      switchMap(_ => this.http.get<Technology[]>(`${environment.apiBaseUrl}/technologies?code=${environment.apiAuthCode}`)),
      tap(_ => this.isLoading$.next(false))
    ).subscribe((technologies: Technology[]) => {
      this.technologies = technologies;
    });
  }

  ngOnInit() {
    this.loadGridData.emit(null);
  }

  ngOnDestroy() {
    if (this.gridDataSub) {
      this.gridDataSub.unsubscribe();
    }
  }

  onAddTechnology() {
    this.modalRef = this.modalService.show(TechnologiesModalComponent, this.modalConfig);

    this.modalRef.content.heading = 'Add new technology';
    this.modalRef.content.technologies = this.technologies;

    this.modalRef.content.technologyData.pipe(
      take(1),
      switchMap((technologyData: Technology) => {
        const url = `${environment.apiBaseUrl}/technologies?code=${environment.apiAuthCode}`;
        return this.http.post<Technology>(url, technologyData);
      })).subscribe((_: any) => this.loadGridData.emit(null));
  }

  openEditTechnologyModal(technology: Technology) {
    this.modalRef = this.modalService.show(TechnologiesModalComponent, this.modalConfig);

    this.modalRef.content.heading = 'Edit technology';
    this.modalRef.content.technologies = this.technologies;
    this.modalRef.content.technology = Object.assign({}, technology);

    this.modalRef.content.technologyData.pipe(
      take(1),
      switchMap((technologyData: Technology) => {
        const url = `${environment.apiBaseUrl}/technologies/${technology.rowKey}?code=${environment.apiAuthCode}`;
        return this.http.put<Technology>(url, technologyData);
      })).subscribe((_: any) => this.loadGridData.emit(null));
  }

  onTechnologyEdit(technology: Technology) {
    const url = `${environment.apiBaseUrl}/technologies/${technology.rowKey}?code=${environment.apiAuthCode}`;
    this.http.get<Technology>(url).pipe(take(1)).subscribe(p => this.openEditTechnologyModal(p));
  }

  onTechnologyDelete(technology: Technology) {
    const url = `${environment.apiBaseUrl}/technologies/${technology.rowKey}?code=${environment.apiAuthCode}`;
    this.http.delete(url).pipe(take(1)).subscribe(_ => this.loadGridData.emit(null));
  }
}
