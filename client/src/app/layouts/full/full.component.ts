import { Component, EventEmitter, Input, Output } from '@angular/core';
import { HomeComponent } from '../../pages/home/home.component';
import { HeaderComponent } from './header/header.component';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from './sidebar/sidebar.component';
import { NavItemComponent } from './sidebar/nav-item/nav-item.component';
import { NavItem } from './sidebar/nav-item/nav-item';
import { navItems, navItemsAdmin } from '../../core/data/sidebar-data';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { CommonModule } from '@angular/common';
import { AppConfigService } from '../../core/services/app-config/app-config.service';
import { AppSetting } from '../../config';
import { LottieComponent, AnimationOptions } from 'ngx-lottie';
import { NbAuthService } from '@nebular/auth';

@Component({
  selector: 'app-full',
  standalone: true,
  imports: [
    HomeComponent,
    HeaderComponent,
    RouterModule,
    SidebarComponent,
    NavItemComponent,
    NgScrollbarModule,
    CommonModule,
    LottieComponent
  ],
  templateUrl: './full.component.html',
  styleUrl: './full.component.css'

})
export class FullComponent {
  navItemsRender: NavItem[] = [];
  navItems: NavItem[] = navItems;
  navItemsAdmin: NavItem[] = navItemsAdmin;
  appSetting: AppSetting;
  showContent: boolean = false;

  roles: string[] = [];

  @Output() isToggled: boolean;

  options: AnimationOptions = {
    path: 'assets/animations/starting.json',
    loop: true,
  };

  options2: AnimationOptions = {
    path: 'assets/animations/ai.json',
    loop: true,
  }

  constructor(
    private _appConfig: AppConfigService,
    private authService: NbAuthService
  ) {
    this.appSetting = this._appConfig.getAppSetting();
    this.setUpAppSetting(this.appSetting);
    this.getUserRole();
    this.checkRole();    
  }
  ngOnInit() {
    this.onShowContent();
  }

  getUserRole() {
    this.authService.onTokenChange().subscribe((token) => {
      if (token?.isValid()) {
        this.roles = token.getPayload()['role'];
      }
    });
  }

  checkRole() {
    if (this.roles.includes('admin')) {
      this.navItemsRender = this.navItemsAdmin;
    }
    else {
      this.navItemsRender = this.navItems;
    }
    console.log(this.navItemsRender);
    
  }

  onShowContent() {
    setTimeout(() => {
      this.showContent = true;
    }, 1500);
  }


  setUpAppSetting(options: AppSetting) {
    this.appSetting = options;
    this._appConfig.setAppSetting(options);
    this.isToggled = this.appSetting.isToggleSidebar;
  }
  
  handleToggle() {
    this.isToggled = !this.isToggled;
    this.appSetting.isToggleSidebar = this.isToggled;
    localStorage.setItem('option_setting_left_sidebar', JSON.stringify(this.appSetting));
  }

}
