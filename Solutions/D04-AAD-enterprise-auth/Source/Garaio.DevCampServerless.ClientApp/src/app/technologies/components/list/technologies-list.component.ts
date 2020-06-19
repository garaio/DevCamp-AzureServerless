import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { Technology, TechnologyType } from '../../../shared/models/technology.model';

@Component({
  selector: 'app-technologies-list',
  templateUrl: './technologies-list.component.html'
})
export class TechnologiesListComponent implements OnInit {
  @Input() technologies: Technology[];
  @Output() technologyDeleted = new EventEmitter<Technology>();
  @Output() technologyEdited = new EventEmitter<Technology>();

  constructor() { }

  ngOnInit() {
  }

  onEdit(technology: Technology) {
    this.technologyEdited.emit(technology);
  }

  onDelete(technology: Technology) {
    this.technologyDeleted.emit(technology);
  }

  trackByFn(index: any) {
    return index;
  }

  getTechnologyTypeName(type: TechnologyType | number): string | undefined {
    return TechnologyType[type];
  }
}
