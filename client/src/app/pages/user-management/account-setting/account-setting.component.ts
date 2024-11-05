import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { TuiBreadcrumbs, TuiTabs } from '@taiga-ui/kit';
import { AccountSettingBreadcrumbData } from './account-setting-breadcrumb-data';
import { Router, RouterModule } from '@angular/router';
import { TuiItem } from '@taiga-ui/cdk';
import { TuiLink } from '@taiga-ui/core';
import { AccountSettingTabsData } from './account-setting-tabs-data';
import { ProfileComponent } from './profile/profile.component';
import { SecurityComponent } from './security/security.component';
import { SocialMediaComponent } from './social-media/social-media.component';

@Component({
  selector: 'app-account-setting',
  standalone: true,
  imports: [
    CommonModule,
    TuiBreadcrumbs,
    RouterModule,
    TuiItem,
    TuiLink,
    TuiTabs,
    ProfileComponent,
    SecurityComponent,
    SocialMediaComponent,
  ],
  templateUrl: './account-setting.component.html',
  styleUrl: './account-setting.component.css'
})
export class AccountSettingComponent {
  breadcrumbs = AccountSettingBreadcrumbData;
  tabs = AccountSettingTabsData;
  constructor(
    private router: Router,
  ) { }
  ngOnInit() { }

  onClickBreadcrumb(routerLink: string) {
    this.router.navigate([routerLink]);
  }

  onClickTab(routerLink: string) {
    this.router.navigate([routerLink]);
  }

}
