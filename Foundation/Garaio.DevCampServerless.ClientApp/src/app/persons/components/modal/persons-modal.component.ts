import { Component, OnInit, ViewChild } from '@angular/core';
import { MDBModalRef } from 'angular-bootstrap-md';
import { Subject } from 'rxjs';
import { NgForm } from '@angular/forms';

import { Project } from '../../../shared/models/project.model';
import { Technology } from 'src/app/shared/models/technology.model';
import { Person } from 'src/app/shared/models/person.model';
import { ProjectExperience } from 'src/app/shared/models/project-experience.model';
import { Skill, SkillLevel } from 'src/app/shared/models/skill.model';

@Component({
  selector: 'app-persons-modal',
  templateUrl: './persons-modal.component.html'
})
export class PersonsModalComponent implements OnInit {
  @ViewChild('personForm', { static: true }) personForm: NgForm;

  heading: string;
  person: Person = {};
  technologies: Technology[];
  projects: Project[];

  skillLevelOptions: string[];

  personData: Subject<Person> = new Subject();

  constructor(public modalRef: MDBModalRef) { }

  ngOnInit() {
    this.skillLevelOptions = Object.keys(SkillLevel).filter(x => !!(Number(x) >= 0));
  }

  getProjectName(projectKey: string): string | undefined {
    if (this.projects) {
      const project = this.projects.find(t => t.rowKey === projectKey);
      return project && `${project.customerName} / ${project.projectName}`;
    }

    return undefined;
  }

  getTechnologyName(technologyKey: string): string | undefined {
    if (this.technologies) {
      const tech = this.technologies.find(t => t.rowKey === technologyKey);
      return tech && tech.name;
    }

    return undefined;
  }

  getSkillLevelName(type: SkillLevel | number): string | undefined {
    return SkillLevel[type];
  }

  onAddProjectExperience(projectKey: string, roleInProject: string, description: string) {
    const projExperience: ProjectExperience = {
      personKey: this.person.rowKey,
      projectKey: projectKey,
      roleInProject: roleInProject,
      description: description
    };

    if (!!this.person.projects) {
      this.person.projects.push(projExperience);
    } else {
      this.person.projects = [ projExperience ];
    }
  }

  onDeleteProjectExperience(projExperience: ProjectExperience) {
    if (this.person.projects) {
      const index = this.person.projects.indexOf(projExperience, 0);
      if (index > -1) {
        this.person.projects.splice(index, 1);
      }
    }
  }

  onAddSkill(technologyKey: string, level: SkillLevel) {
    const skill: Skill = {
      personKey: this.person.rowKey,
      technologyKey: technologyKey,
      level: level
    };

    if (!!this.person.skills) {
      this.person.skills.push(skill);
    } else {
      this.person.skills = [ skill ];
    }
  }

  onDeleteSkill(skill: Skill) {
    if (this.person.skills) {
      const index = this.person.skills.indexOf(skill, 0);
      if (index > -1) {
        this.person.skills.splice(index, 1);
      }
    }
  }

  onSave() {
    if (this.personForm.valid) {
      this.personData.next(this.person);
    this.modalRef.hide();
    } else {
      const controls = this.personForm.controls;
      Object.keys(controls).forEach( controlName => controls[controlName].markAsTouched());
    }
  }
}
