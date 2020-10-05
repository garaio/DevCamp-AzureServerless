import { Component, OnInit, OnDestroy, EventEmitter } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MDBModalRef, MDBModalService } from 'angular-bootstrap-md';
import { Subscription, Subject } from 'rxjs';
import { take, switchMap, tap } from 'rxjs/operators';

import { Project } from '../../shared/models/project.model';
import { ProjectsModalComponent } from '../components/modal/projects-modal.component';
import { Technology } from 'src/app/shared/models/technology.model';
import { AppConfigService } from 'src/app/app-config.service';

@Component({
  selector: 'app-projects',
  templateUrl: './projects.component.html',
  styleUrls: ['./projects.component.scss']
})
export class ProjectsComponent implements OnInit, OnDestroy {
  isLoading$: Subject<boolean> = new Subject<boolean>();

  technologies: Technology[];
  projects: Project[] | null;
  modalRef: MDBModalRef;

  gridDataSub: Subscription;
  loadGridData: EventEmitter<any> = new EventEmitter();

  modalConfig = {
        class: 'modal-dialog modal-dialog-scrollable'
  };

  constructor(private modalService: MDBModalService, private http: HttpClient, private appConfig: AppConfigService) {
    this.gridDataSub = this.loadGridData.pipe(
      tap(_ => this.isLoading$.next(true)),
      switchMap(_ => this.http.get<Project[]>(`${this.appConfig.get().api.baseUrl}/projects?code=${this.appConfig.get().api.authCode}`)),
      tap(_ => this.isLoading$.next(false))
    ).subscribe((projects: Project[]) => {
      this.projects = projects;
    });
  }

  ngOnInit() {
    this.loadGridData.emit(null);

    this.http.get<Technology[]>(`${this.appConfig.get().api.baseUrl}/technologies?code=${this.appConfig.get().api.authCode}`)
    .pipe(take(1))
    .subscribe(t => this.technologies = t);
  }

  ngOnDestroy() {
    if (this.gridDataSub) {
      this.gridDataSub.unsubscribe();
    }
  }

  onAddProject() {
    this.modalRef = this.modalService.show(ProjectsModalComponent, this.modalConfig);

    this.modalRef.content.heading = 'Add new project';
    this.modalRef.content.technologies = this.technologies;

    this.modalRef.content.projectData.pipe(
      take(1),
      switchMap((projectData: Project) => {
        const url = `${this.appConfig.get().api.baseUrl}/projects?code=${this.appConfig.get().api.authCode}`;
        return this.http.post<Project>(url, projectData);
      })).subscribe((_: any) => this.loadGridData.emit(null));
  }

  openEditProjectModal(project: Project) {
    this.modalRef = this.modalService.show(ProjectsModalComponent, this.modalConfig);

    this.modalRef.content.heading = 'Edit project';
    this.modalRef.content.technologies = this.technologies;
    this.modalRef.content.project = Object.assign({}, project);

    this.modalRef.content.projectData.pipe(
      take(1),
      switchMap((projectData: Project) => {
        const url = `${this.appConfig.get().api.baseUrl}/projects/${project.entityKey}?code=${this.appConfig.get().api.authCode}`;
        return this.http.put<Project>(url, projectData);
      })).subscribe((_: any) => this.loadGridData.emit(null));
  }

  onProjectEdit(project: Project) {
    const url = `${this.appConfig.get().api.baseUrl}/projects/${project.entityKey}?code=${this.appConfig.get().api.authCode}`;
    this.http.get<Project>(url).pipe(take(1)).subscribe(p => this.openEditProjectModal(p));
  }

  onProjectDelete(project: Project) {
    const url = `${this.appConfig.get().api.baseUrl}/projects/${project.entityKey}?code=${this.appConfig.get().api.authCode}`;
    this.http.delete(url).pipe(take(1)).subscribe(_ => this.loadGridData.emit(null));
  }
}
