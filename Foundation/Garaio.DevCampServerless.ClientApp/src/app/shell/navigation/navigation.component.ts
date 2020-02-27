import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit, OnDestroy {
  @ViewChild('sidenav', {static: true}) sidenav: ElementRef;

  clicked: boolean;

  userName$: Observable<string>;

  constructor(private http: HttpClient) {
    this.clicked = this.clicked === undefined ? false : true;
  }

  ngOnInit() {
    const url = `${environment.apiBaseUrl}/user?code=${environment.apiAuthCode}`;
    this.userName$ = this.http.get(url, {responseType: 'text'});
  }

  ngOnDestroy() {
  }

  setClicked(val: boolean): void {
    this.clicked = val;
  }

}
