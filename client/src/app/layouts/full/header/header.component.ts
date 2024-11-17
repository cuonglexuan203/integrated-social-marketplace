import { Component, ElementRef, EventEmitter, inject, Input, Output, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { Placeholder } from '../../../core/enums/placehoder';
import { BrandingComponent } from '../sidebar/branding.component';
import { TooltipHeaderAction } from '../../../core/enums/tooltip';
import { TuiDataList, TuiDropdown, TuiIcon } from '@taiga-ui/core';
import { AlertService } from '../../../core/services/alert/alert.service';
import { TuiAvatar, TuiBadge, TuiBadgedContent, TuiBadgeNotification } from '@taiga-ui/kit';
import { UserDialogComponent } from '../../../shared/components/user-dialog/user-dialog.component';
import { NbAuthService } from '@nebular/auth';
import { UserService } from '../../../core/services/user/user.service';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { Helper } from '../../../core/utils/helper';
import { Router } from '@angular/router';

import {WA_LOCAL_STORAGE, WA_WINDOW} from '@ng-web-apis/common';
import {TUI_DARK_MODE, TUI_DARK_MODE_KEY, TuiButton, TuiOption} from '@taiga-ui/core';
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
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  user: UserResponseModel;
  userId: string;
  @ViewChild('inputSearch') inputSearch: ElementRef;
  @Output() toggleSidebar = new EventEmitter<void>();
  placeholder = Placeholder;
  tooltipHeaderAction = TooltipHeaderAction;


  private readonly key = inject(TUI_DARK_MODE_KEY);
  private readonly storage = inject(WA_LOCAL_STORAGE);
  private readonly media = inject(WA_WINDOW).matchMedia('(prefers-color-scheme: dark)');
  protected readonly darkMode = inject(TUI_DARK_MODE);
  
  constructor(
    private alertService: AlertService,
    private authService: NbAuthService,
    private _userService: UserService,
    private router: Router
  ) {
    this.getUserToken();
    this.getUserDetail();
  }

  ngOnInit(
  ) { }

  focusSearchInput() {
    this.inputSearch.nativeElement.focus();
  }


  onToggleSidebar() {
    this.toggleSidebar.emit();
  }


  setDarkMode() {
    this.darkMode.set(!this.darkMode())
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
        if(res) {
          this.user = res;
        }
      },
      error: (error) => {
        this.alertService.showError('Error', error);
      },
      complete : () => {

      }
    })
  }

  logOut() {
    this.authService.logout('email').subscribe(result => {
      if(result.isSuccess()) {
        this.alertService.showSuccess('Success','Logout Successfully');
        Helper.clearLocalStorage();
        this.router.navigate(['/login']);
      }
    })
  }

}
