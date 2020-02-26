import { Component, OnInit, ViewChild } from '@angular/core';
import { MDBModalRef } from 'angular-bootstrap-md';
import { Subject } from 'rxjs';
import { NgForm } from '@angular/forms';

import { Project } from '../../../shared/models/project.model';
import { Technology } from 'src/app/shared/models/technology.model';
import { ProjectTechnology } from 'src/app/shared/models/project-technology.model';

@Component({
  selector: 'app-projects-modal',
  templateUrl: './projects-modal.component.html'
})
export class ProjectsModalComponent implements OnInit {
  @ViewChild('projectForm', { static: true }) projectForm: NgForm;

  heading: string;
  project: Project = {};
  technologies: Technology[];

  projectData: Subject<Project> = new Subject();

  constructor(public modalRef: MDBModalRef) { }

  ngOnInit() {
  }

  getTechnologyName(technologyKey: string): string | undefined {
    if (this.technologies) {
      const tech = this.technologies.find(t => t.rowKey === technologyKey);
      return tech && tech.name;
    }

    return undefined;
  }

  onAddTechnology(technologyKey: string, component: string) {
    const projTechnology: ProjectTechnology = {
      technologyKey: technologyKey,
      component: component
    };

    if (!!this.project.usedTechnologies) {
      this.project.usedTechnologies.push(projTechnology);
    } else {
      this.project.usedTechnologies = [ projTechnology ];
    }
  }

  onDeleteTechnology(projTechnology: ProjectTechnology) {
    if (this.project.usedTechnologies) {
      const index = this.project.usedTechnologies.indexOf(projTechnology, 0);
      if (index > -1) {
        this.project.usedTechnologies.splice(index, 1);
      }
    }
  }

  onSave() {
    if (this.projectForm.valid) {
      this.projectData.next(this.project);
    this.modalRef.hide();
    } else {
      const controls = this.projectForm.controls;
      Object.keys(controls).forEach( controlName => controls[controlName].markAsTouched());
    }
  }
}
