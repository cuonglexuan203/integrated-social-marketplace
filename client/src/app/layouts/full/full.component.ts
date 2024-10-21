import { Component, EventEmitter, Input, Output } from '@angular/core';
import { HomeComponent } from '../../pages/home/home.component';
import { HeaderComponent } from './header/header.component';
import { RouterModule } from '@angular/router';
import { SidebarComponent } from './sidebar/sidebar.component';
import { NavItemComponent } from './sidebar/nav-item/nav-item.component';
import { NavItem } from './sidebar/nav-item/nav-item';
import { navItems } from './sidebar/sidebar-data';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { CommonModule } from '@angular/common';
import { AppConfigService } from '../../core/services/app-config/app-config.service';
import { AppSetting } from '../../config';
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
    CommonModule
  ],
  templateUrl: './full.component.html',
  styleUrl: './full.component.css'

})
export class FullComponent {
  navItems: NavItem[] = navItems;
  appSetting: AppSetting;
  @Output() isToggled: boolean;
  constructor(
    private _appConfig: AppConfigService,
  ) {
    this.appSetting = this._appConfig.getAppSetting();
    this.setUpAppSetting(this.appSetting);
  }
  ngOnInit() {
    
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
