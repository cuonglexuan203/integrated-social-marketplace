import { Component, HostListener, inject, INJECTOR, Output } from '@angular/core';
import { UserNewPostComponent } from '../../home/user-new-post/user-new-post.component';
import { TuiAvatar, TuiSkeleton } from '@taiga-ui/kit';
import { TuiDialogService, TuiIcon } from '@taiga-ui/core';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { Helper } from '../../../core/utils/helper';
import { PostItemComponent } from '../../home/post-item/post-item.component';
import { AlertService } from '../../../core/services/alert/alert.service';
import { FeedService } from '../../../core/services/feed/feed.service';
import { FeedPost } from '../../../core/models/feed/feed.model';
import { Page, PageUserDetail } from '../../../core/models/page/page.model';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, NavigationEnd, ParamMap, Router } from '@angular/router';
import { UserService } from '../../../core/services/user/user.service';
import { lastValueFrom, Subscription } from 'rxjs';
import { UserStateService } from '../../../core/services/state/user-state/user-state.service';
import { PostStateService } from '../../../core/services/state/post-state/post-state.service';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { CreatePostDialogComponent } from '../../../shared/components/create-post-dialog/create-post-dialog.component';

@Component({
  selector: 'app-user-profile',
  standalone: true,
  imports: [
    UserNewPostComponent,
    TuiAvatar,
    TuiIcon,
    PostItemComponent,
    CommonModule,
    TuiSkeleton,
  ],
  templateUrl: './user-profile.component.html',
  styleUrl: './user-profile.component.css'
})
export class UserProfileComponent {

  private readonly injector = inject(INJECTOR);
  private readonly dialogs = inject(TuiDialogService);

  @Output() posts: FeedPost[] = [];
  userLoggedIn: UserResponseModel;

  user: UserResponseModel;
  userName: string;

  page: PageUserDetail;

  isLoading: boolean = false;
  isCheckingUserLoggedIn: boolean = false;

  loadingPosts: FeedPost[] = [];
  isAllPostsLoaded = false;

  private paramMapSubscription: Subscription;

  constructor(
    private alertService: AlertService,
    private _feedService: FeedService,
    private activatedRoute: ActivatedRoute,
    private _userService: UserService,
    private router: Router,
    private _userStateService: UserStateService,
    private _postStateService: PostStateService
  ) {
  }


  ngOnInit() {
    this.setUpData();
  }


  async setUpData() {
    this.getUserLoggedIn();
    await this.getUserFromUserName();
  }

  getUserLoggedIn() {
    this.userLoggedIn = Helper.getUserFromLocalStorage();
  }

  
  setupPage() {
    this.page = {
      pageIndex: 1,
      pageSize: 5,
      sort: 'asc',
      userId: this.isCheckingUserLoggedIn ? this.userLoggedIn.id : this.user.id
    }
  }

  async checkingUserLoggedIn() {
    if (this.userLoggedIn && this.user) {
      if (this.userLoggedIn.id === this.user.id) {
        this.isCheckingUserLoggedIn = true;
      }
    }
  }

  getStateFilter() {
    return this._userStateService.getStateFilter();
  }

  ngOnDestroy() {
    if (this.paramMapSubscription) {
      this.paramMapSubscription.unsubscribe();
    }

    this._userStateService.setStateFilter('All');
  }

  resetData() {
    this.user = new UserResponseModel();
    this.isCheckingUserLoggedIn = false;
  }

  async getUserFromUserName() {
    this.paramMapSubscription = this.activatedRoute.paramMap.subscribe(async (params: ParamMap) => {
      const newUsername = params.get('userName') || '';

      if (newUsername !== this.userName) {
        this.userName = newUsername;
        this.resetData();
        
        // Combine the calls into a single method
        await this.loadUserProfileData();
      }
    });
  }

  async loadUserProfileData() {
    if (this.userName) {
      try {
        // Load user details
        await this.getUserDetailByUserName(this.userName);
        this.setupPage();
        
        // Load posts after user details are fetched
        await this.getPosts();
      } catch (error) {
        console.error('Error loading user profile data:', error);
        this.alertService.showError('Failed to load user profile', 'Error');
      }
    }
  }

  async getUserDetailByUserName(userName: string): Promise<void> {
    return new Promise((resolve, reject) => {
      this.isLoading = true;
      this._userService.getUserDetailByUserName(userName).subscribe({
        next: (res) => {
          if (res) {
            this.user = res?.result;
            console.log(this.user);

          }
          this.isLoading = false;
          resolve();
        },
        error: (error) => {
          console.error(error);
          this.isLoading = false;
          reject(error);
        },
        complete: async () => {
          await this.checkingUserLoggedIn();
        }
      });
    });
  }

  @HostListener('window:scroll', ['$event.target'])
  onWindowScroll(event: any) {
    // Only trigger if not all posts are loaded and not currently loading
    if (
      (window.innerHeight + window.scrollY >= document.body.offsetHeight && 
      !this.isLoading && 
      !this.isAllPostsLoaded && !(this.getStateFilter() === 'SavedPosts') && !(this.getStateFilter() === 'CommentedPosts') && !(this.getStateFilter() === 'ReactedPosts'))
    ) {
      this.getPosts();
    }
  }


  async getPosts() {

    // Reset posts if the filter is changed
    if (this.getStateFilter() === 'SavedPosts' || this.getStateFilter() === 'CommentedPosts' || this.getStateFilter() === 'ReactedPosts') {
      this.resetPostLoading();
    }


    this.isLoading = true;
    this._userStateService.setStateFilter('All')
    this._feedService.getPostByUserId(this.page).subscribe({
      next: (res) => {
        if (res) {
          if (!res?.result?.data.length) {
            this.isAllPostsLoaded = true;
            this.isLoading = false;
            this.loadingPosts = [];
            return;
          }
          this.posts.push(...res?.result?.data);
          this.loadingPosts = res?.result?.data;
          this.isLoading = false;
        }
      },
      error: (error) => {
        this.isLoading = false;
        this.isAllPostsLoaded = true;
        console.error(error);
      },
      complete: () => {
        if (!this.isAllPostsLoaded) {
          this.page = {
            pageIndex: this.page.pageIndex + 1,
            pageSize: 5,
            sort: 'asc',
            userId: this.isCheckingUserLoggedIn ? this.userLoggedIn.id : this.user.id
          };
        }
        
        this.loadingPosts = [];
      }
    });
  }


  resetPostLoading() {
    this.isAllPostsLoaded = false;
    this.page.pageIndex = 1;
    this.posts = [];
  }

  filterSavedPosts() {
    this.isAllPostsLoaded = true;
    this._userStateService.setStateFilter('SavedPosts')
    this.isLoading = true;
    this._feedService.getSavedPostByUserId(this.user.id).subscribe({
      next: (res) => {
        if (res) {
          this.posts = res?.result || [];
          this._postStateService.setSavedPosts(this.posts);
          this.isLoading = false;
        }
      },
      error: (error) => {
        console.error(error);
        this.isLoading = false;
      },
      complete: () => {
        this.isLoading = false;
      }

    });
  }

  createNewPost () {

    const data = { title: 'Create Post' };
    this.dialogs.open(
      new PolymorpheusComponent(CreatePostDialogComponent, this.injector),
      {
        data: data,
        dismissible: false,
      }
    ).subscribe({
      next: (data: any) => {
        this.posts.push(data);
      },
      error: (error) => {
        console.error(error);
      },
      complete: () => {

      }
    })
  }

  filterCommentedPosts() {
    this.isAllPostsLoaded = true;
    this.posts = [];
    this._userStateService.setStateFilter('CommentedPosts')
  }

  filterReactedPost() {
    this.isAllPostsLoaded = true;
    this.posts = [];

    this._userStateService.setStateFilter('ReactedPosts')
  }

  editUserProfile() {
    this.router.navigate(['/user/account-settings/profile']);
  }

  backToTop() {
    window.scrollTo(0, 0);
  }


}
