import { Component, HostListener, Output } from '@angular/core';
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
import { Page } from '../../core/models/page/page.model';
import { NbAuthService } from '@nebular/auth';
import { UserService } from '../../core/services/user/user.service';
import { UserResponseModel } from '../../core/models/user/user.model';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    PostItemComponent,
    TagItemComponent,
    UserNewPostComponent,
    TuiSkeleton,
    CommonModule,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  @Output() posts: FeedPost[] = [];
  loadingPosts: FeedPost[] = [];
  isLoading: boolean = false;

  newPostCreated: FeedPost;

  page: Page = {
    pageIndex: 1,
    pageSize: 5,
    sort: 'asc',
  }

  constructor(
    private _feedService: FeedService,
    private alertService: AlertService,
  ) {
  }

  ngOnInit() {
    this.getAllPosts();
  }

  checkNewPostCreated(event: any) {
    if (event) {
      this.newPostCreated = event;
      this.posts = [this.newPostCreated, ...this.posts]; // Update reference
    }
  }

  @HostListener('window:scroll', ['$event.target'])
  onWindowScroll(event: any) {
    if (window.innerHeight + window.scrollY >= document.body.offsetHeight && !this.isLoading) {
      this.getAllPosts()
    }
  }

  getAllPosts() {
    this.isLoading = true;
    this._feedService.getPosts(this.page).subscribe({
      next: (res) => {
        if (res) {
          this.posts.push(...res?.result?.data);
          this.loadingPosts = res?.result?.data;
          this.isLoading = false;
        }
        else {
          this.isLoading = false;
        }
      },
      error: (err) => {
        this.isLoading = false;
        this.alertService.showError('Get All Posts Fail', err.error.message);
      },
      complete: () => {
        this.isLoading = false;
        this.page = {
          pageIndex: this.page.pageIndex + 1,
          pageSize: 10,
          sort: 'asc',
        }
        this.loadingPosts = [];
      }
    });
  }

  





}
