import { Component, OnInit, OnDestroy, ViewChild, ElementRef } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

import { AppConfigService } from 'src/app/app-config.service';

@Component({
  selector: 'app-navigation',
  templateUrl: './navigation.component.html',
  styleUrls: ['./navigation.component.scss']
})
export class NavigationComponent implements OnInit, OnDestroy {
  @ViewChild('sidenav', {static: true}) sidenav: ElementRef;

  clicked: boolean;

  userName$: Observable<string>;

  constructor(private http: HttpClient, private appConfig: AppConfigService) {
    this.clicked = this.clicked === undefined ? false : true;
  }

  ngOnInit() {
    const url = `${this.appConfig.get().api.baseUrl}/user?code=${this.appConfig.get().api.authCode}`;
    this.userName$ = this.http.get(url, {responseType: 'text'});
  }

  ngOnDestroy() {
  }

  setClicked(val: boolean): void {
    this.clicked = val;
  }

}
