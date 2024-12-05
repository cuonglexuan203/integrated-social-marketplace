import { Component, ElementRef, EventEmitter, inject, INJECTOR, Input, Output, TemplateRef, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { Placeholder } from '../../../core/enums/placehoder';
import { BrandingComponent } from '../sidebar/branding.component';
import { TooltipHeaderAction } from '../../../core/enums/tooltip';
import { TuiDataList, TuiDialogService, TuiDriver, TuiDriverDirective, TuiDropdown, TuiDropdownDriver, TuiDropdownDriverDirective, TuiIcon } from '@taiga-ui/core';
import { AlertService } from '../../../core/services/alert/alert.service';
import { TuiAvatar, TuiBadge, TuiBadgedContent, TuiBadgeNotification, TuiSkeleton } from '@taiga-ui/kit';
import { UserDialogComponent } from '../../../shared/components/user-dialog/user-dialog.component';
import { NbAuthService } from '@nebular/auth';
import { UserService } from '../../../core/services/user/user.service';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { Helper } from '../../../core/utils/helper';
import { Router } from '@angular/router';
import { LottieComponent, AnimationOptions } from 'ngx-lottie';
import { FormsModule } from '@angular/forms';
import { debounceTime, Subject } from 'rxjs';
import { FeedService } from '../../../core/services/feed/feed.service';
import { CommentPostDialogComponent } from '../../../shared/components/comment-post-dialog/comment-post-dialog.component';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [
    CommonModule,
    NgScrollbarModule,
    BrandingComponent,
    TuiIcon,
    TuiAvatar,
    TuiBadge,
    TuiBadgedContent,
    TuiBadgeNotification,
    TuiDropdown,
    TuiDataList,
    UserDialogComponent,
    LottieComponent,
    FormsModule,
    TuiSkeleton
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  private readonly injector = inject(INJECTOR);
  private readonly dialogs = inject(TuiDialogService);

  @ViewChild('searchDialog') searchDialog: TemplateRef<any>;

  user: UserResponseModel;
  userId: string;

  @ViewChild('inputSearch') inputSearch: ElementRef;
  @ViewChild('inputSearch') dropdownSearchValueElement: ElementRef;

  @Output() toggleSidebar = new EventEmitter<void>();

  placeholder = Placeholder;
  tooltipHeaderAction = TooltipHeaderAction;


  options: AnimationOptions = {
    path: 'assets/animations/logo.json',
    loop: true,
  };

  searchValue = '';
  listSelectionSearch = [
    {
      value: '1',
      label: 'Post',
    },
    {
      value: '2',
      label: 'User',
    },
  ]

  selectedSearch = this.listSelectionSearch[0];
  private searchSubject: Subject<any> = new Subject<any>();

  dropdownSearchValue: any[] = [];
  dropdownSearchValueUser: any[] = [];
  isLoading: boolean = false;

  constructor(
    private alertService: AlertService,
    private authService: NbAuthService,
    private _userService: UserService,
    private router: Router,
    private _feedService: FeedService
  ) {
    this.getUserToken();
    this.getUserDetail();
  }

  ngOnInit(
  ) {
    this.searchSubject.pipe(debounceTime(500)).subscribe(() => {
      this.onOpenDropdown();
    });
  }

  onSearchTyping() {
    this.searchSubject.next(this.searchValue);
    if (!this.searchValue) {
      this.dropdownSearchValue = [];
      this.dropdownSearchValueUser = [];
    }

  }

  onOpenDropdown() {
    if (this.selectedSearch) {
      if (this.searchValue && this.selectedSearch.value === '1') {
        this.isLoading = true;
        this._feedService.searchPosts(this.searchValue).subscribe({
          next: (res) => {
            this.dropdownSearchValue = res?.result;
            this.isLoading = false;
          },
          error: (error) => {
            this.isLoading = false;
            this.alertService.showError('Error', error);
          },
          complete: () => {
            this.isLoading = false;
          }
        });
      }
      if (this.searchValue && this.selectedSearch.value === '2') {
        this.isLoading = true;
        this._userService.searchUserFullName(this.searchValue).subscribe({
          next: (res) => {
            this.dropdownSearchValueUser = res?.result?.data;
            this.isLoading = false;
          },
          error: (error) => {
            this.isLoading = false;
            this.alertService.showError('Error', error);
          },
          complete: () => {
            this.isLoading = false;
          }
        });
      }
    }
  }

  onSelectedDropdownSearch(item: any) {
    if (this.selectedSearch.value === '1') {
      const data = { title: 'Comment', post: item };
      this.dialogs
        .open(
          new PolymorpheusComponent(CommentPostDialogComponent, this.injector),
          {
            dismissible: false,
            size: 'auto',
            data: data,
          }
        ).subscribe({
          next: (data) => {
            console.log(data);
          },
          error: (error) => {
            console.error(error);
          },
          complete: () => {
          }
        })
        ;
    }
    else {
      this.router.navigate(['/user/user-profile', item?.userName]);
    }

  }



  ngOnDestroy() {
    this.searchSubject.complete();
  }

  focusSearchInput() {
    this.inputSearch.nativeElement.focus();
  }


  onToggleSidebar() {
    this.toggleSidebar.emit();
  }

  getUserToken() {
    this.authService.onTokenChange().subscribe((token) => {
      if (token?.isValid()) {
        this.userId = token?.getPayload()?.userId;
      }
    });
  }

  getUserDetail() {
    this._userService.getUserDetail(this.userId).subscribe({
      next: (res) => {
        if (res) {
          this.user = res.result;
          localStorage.setItem('user', JSON.stringify(this.user));
        }
      },
      error: (error) => {
        this.alertService.showError('Error', error);
      },
      complete: () => {

      }
    })
  }

  logOut() {
    this.authService.logout('email').subscribe(result => {
      if (result.isSuccess()) {
        this.alertService.showSuccess('Success', 'Logout Successfully');
        Helper.clearLocalStorage();
        this.router.navigate(['/login']);
      }
    })
  }

  onSearch() {
    if (this.searchValue) {
      if (this.selectedSearch.value === '1') {
        this.router.navigate(['/search'], { queryParams: { post: this.searchValue } });
      } else {
        this.router.navigate(['/search'], { queryParams: { user: this.searchValue } });
      }
    }
  }

  onSelectedSearch(item: any) {
    this.selectedSearch = item;
    this.dropdownSearchValue = [];
    this.dropdownSearchValueUser = [];
    this.searchValue = '';
    this.searchDialog.elementRef.nativeElement.close();

  }

}
