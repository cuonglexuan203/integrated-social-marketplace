import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { FeedService } from '../../core/services/feed/feed.service';
import { FeedPost } from '../../core/models/feed/feed.model';
import { PostItemComponent } from '../home/post-item/post-item.component';
import { TuiSkeleton } from '@taiga-ui/kit';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-search-result',
  standalone: true,
  imports: [
    PostItemComponent,
    TuiSkeleton,
  ],
  templateUrl: './search-result.component.html',
  styleUrl: './search-result.component.css'
})
export class SearchResultComponent {
  constructor(
    private activatedRoute: ActivatedRoute,
    private _feedService: FeedService,

  ) { }

  searchValue: any;
  isLoading = false;
  postSearchResult: FeedPost[] = [];

  private _onDestroy = new Subject<void>();

  ngOnInit() {
    this.handleGetParams();
    this.handleSearch();
  }

  handleGetParams() {
    this.activatedRoute.queryParams.pipe(takeUntil(this._onDestroy)).subscribe(params => {
      const searchValue = params;
      if (searchValue != this.searchValue) {
        this.searchValue = searchValue;
        this.handleSearch();
      }
    })

  }

  handleSearch() {
    if (this.searchValue) {
      if (this.searchValue?.post) {
        this.isLoading = true;
        this._feedService.searchPosts(this.searchValue.post).subscribe({
          next: (res) => {
            this.postSearchResult = res?.result;
          },
          error: (err) => {
            this.isLoading = false;
            console.log(err);
          },
          complete: () => {
            this.isLoading = false;
          }
        })
      }
      else {

      }
    }
  }

}
