import { Component, OnInit, EventEmitter } from '@angular/core';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Subject, Observable } from 'rxjs';
import { tap, switchMap, map } from 'rxjs/operators';

import { AppConfigService } from 'src/app/app-config.service';
import { SearchResult, SearchResultType } from 'src/app/shared/models/search-result.model';

@Component({
  selector: 'app-overview',
  templateUrl: './overview.component.html',
  styleUrls: ['./overview.component.scss']
})
export class OverviewComponent implements OnInit {
  isLoading$: Subject<boolean> = new Subject<boolean>();
  hasResults$: Subject<boolean> = new Subject<boolean>();

  searchResults$: Observable<SearchResult[]>;
  search: EventEmitter<string> = new EventEmitter();

  constructor(private http: HttpClient, private router: Router, private appConfig: AppConfigService) {
    const url = `${this.appConfig.get().api.baseUrl}/search?code=${this.appConfig.get().api.authCode}`;

    this.searchResults$ = this.search.pipe(
      tap(_ => this.isLoading$.next(true)),
      switchMap(query => this.http.post<SearchResult[]>(url, query)
                                  .pipe(map(results => ({ results, count: results && results.length, query })))),
      map(result => {
        if (result.count === 0 && result.query.toUpperCase() === 'TEST'.toUpperCase()) {
          return [
            { resultKey: '868f0852-cbbe-44fc-a967-b6d34d410e25', resultText: 'Dummy Search Result', type: SearchResultType.Entity },
            { resultKey: 'ListProjects', resultText: 'Show Projects', type: SearchResultType.Intend },
            { resultKey: 'ListPersons', resultText: 'Show Persons', type: SearchResultType.Intend },
            { resultKey: 'ListTechnologies', resultText: 'Show Technologies', type: SearchResultType.Intend }
          ];
        }

        return result.results;
      }),
      tap(results => {
        this.hasResults$.next(results && results.length > 0);
        this.isLoading$.next(false);
      })
    );
  }

  ngOnInit() {
  }

  onSearch(query: string) {
    if (query) {
      this.search.next(query);
    }
  }

  onOpenSearchResult(result: SearchResult) {
    if (result && result.type === SearchResultType.Intend) {

      switch (result.resultKey) {
        case 'ListPersons': {
          this.router.navigate(['/persons']);
          break;
        }
        case 'ListProjects': {
          this.router.navigate(['/projects']);
          break;
        }
        case 'ListTechnologies': {
          this.router.navigate(['/technologies']);
          break;
        }
        default: {
          console.log(result.resultKey);
          break;
        }
      }
    }
  }

  getSearchResultTypeName(type: SearchResultType | number): string | undefined {
    return SearchResultType[type];
  }
}
