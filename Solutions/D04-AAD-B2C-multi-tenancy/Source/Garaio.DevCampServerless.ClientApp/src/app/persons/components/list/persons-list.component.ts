import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

import { PublishState } from 'src/app/shared/models/publish-state.model';
import { Person } from 'src/app/shared/models/person.model';

@Component({
  selector: 'app-persons-list',
  templateUrl: './persons-list.component.html'
})
export class PersonsListComponent implements OnInit {
  @Input() persons: Person[];
  @Output() personDeleted = new EventEmitter<Person>();
  @Output() personEdited = new EventEmitter<Person>();

  constructor() { }

  ngOnInit() {
  }

  onEdit(person: Person) {
    this.personEdited.emit(person);
  }

  onDelete(person: Person) {
    this.personDeleted.emit(person);
  }

  trackByFn(index: any) {
    return index;
  }

  getStatusName(status: PublishState | number): string | undefined {
    return PublishState[status];
  }
}
