import { Component, Output } from '@angular/core';
import { PostItemComponent } from "./post-item/post-item.component";
import { MockDataPost } from '../../shared/mocks/mock-data-post';
import { FilterComponent } from '../../shared/components/filter/filter.component';
import { FilterData } from '../../core/data/filter-data';
import { Filter } from '../../core/models/filter/filter.model';
import { AlertService } from '../../core/services/alert/alert.service';
import { FeedService } from '../../core/services/feed/feed.service';
import { FeedPost } from '../../core/models/feed/feed.model';
import { TuiSkeleton } from '@taiga-ui/kit';
import { TagItemComponent } from './tag-item/tag-item.component';
import { UserNewPostComponent } from './user-new-post/user-new-post.component';
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    PostItemComponent,
    TagItemComponent,
    UserNewPostComponent,
    TuiSkeleton,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  @Output() posts: FeedPost[] = [];
  isLoading: boolean = false;
  constructor(
    private _feedService: FeedService,
    private alertService: AlertService,
  ) { }

  ngOnInit() {
    this.getAllPosts();
    
  }

  ngOnChanges() {
  }

  getAllPosts() {
    this.isLoading = true;
    this._feedService.getAllPosts().subscribe({
      next: (res) => {
        if (res) {
          this.posts = res.result;
          this.isLoading = false;
        }
        else {
          this.isLoading = false;
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.alertService.showError('Get All Posts Fail' ,err.error.message);
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }



}
