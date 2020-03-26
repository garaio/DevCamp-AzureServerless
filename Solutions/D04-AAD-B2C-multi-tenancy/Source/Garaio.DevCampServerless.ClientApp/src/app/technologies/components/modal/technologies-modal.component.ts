import { Component, OnInit, ViewChild } from '@angular/core';
import { MDBModalRef } from 'angular-bootstrap-md';
import { Subject } from 'rxjs';
import { NgForm } from '@angular/forms';

import { Technology, TechnologyType } from 'src/app/shared/models/technology.model';
import { TechnologyLink, LinkType } from 'src/app/shared/models/technology-link.model';

@Component({
  selector: 'app-technologies-modal',
  templateUrl: './technologies-modal.component.html'
})
export class TechnologiesModalComponent implements OnInit {
  @ViewChild('technologyForm', { static: true }) technologyForm: NgForm;

  heading: string;
  technology: Technology = {};
  technologies: Technology[];

  linkTypeOptions: string[];
  technologyOptions: string[];

  technologyData: Subject<Technology> = new Subject();

  constructor(public modalRef: MDBModalRef) { }

  ngOnInit() {
    this.linkTypeOptions = Object.keys(LinkType).filter(x => !!(Number(x) >= 0));
    this.technologyOptions = Object.keys(TechnologyType).filter(x => !!(Number(x) >= 0));
  }

  getTechnologyName(technologyKey: string): string | undefined {
    if (this.technologies) {
      const tech = this.technologies.find(t => t.rowKey === technologyKey);
      return tech && tech.name;
    }

    return undefined;
  }

  getTechnologyTypeName(type: TechnologyType | number): string | undefined {
    return TechnologyType[type];
  }

  getLinkTypeName(type: LinkType | number): string | undefined {
    return LinkType[type];
  }

  onAddTechnologyLink(technologyKey: string, type: number | LinkType) {
    const technologyLink: TechnologyLink = {
      fromTechnologyKey: this.technology.rowKey,
      toTechnologyKey: technologyKey,
      type: type
    };

    if (!!this.technology.linkedTechnologies) {
      this.technology.linkedTechnologies.push(technologyLink);
    } else {
      this.technology.linkedTechnologies = [ technologyLink ];
    }
  }

  onDeleteTechnologyLink(technologyLink: TechnologyLink) {
    if (this.technology.linkedTechnologies) {
      const index = this.technology.linkedTechnologies.indexOf(technologyLink, 0);
      if (index > -1) {
        this.technology.linkedTechnologies.splice(index, 1);
      }
    }
  }

  onSave() {
    if (this.technologyForm.valid) {
      this.technologyData.next(this.technology);
    this.modalRef.hide();
    } else {
      const controls = this.technologyForm.controls;
      Object.keys(controls).forEach( controlName => controls[controlName].markAsTouched());
    }
  }
}
