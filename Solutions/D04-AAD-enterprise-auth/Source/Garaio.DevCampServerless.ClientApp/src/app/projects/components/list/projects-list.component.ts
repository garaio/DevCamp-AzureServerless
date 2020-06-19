import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';
import { Project } from '../../../shared/models/project.model';
import { PublishState } from 'src/app/shared/models/publish-state.model';

@Component({
  selector: 'app-projects-list',
  templateUrl: './projects-list.component.html'
})
export class ProjectsListComponent implements OnInit {
  @Input() projects: Project[];
  @Output() projectDeleted = new EventEmitter<Project>();
  @Output() projectEdited = new EventEmitter<Project>();

  constructor() { }

  ngOnInit() {
  }

  onEdit(project: Project) {
    this.projectEdited.emit(project);
  }

  onDelete(project: Project) {
    this.projectDeleted.emit(project);
  }

  trackByFn(index: any) {
    return index;
  }

  getStatusName(status: PublishState | number): string | undefined {
    return PublishState[status];
  }
}
