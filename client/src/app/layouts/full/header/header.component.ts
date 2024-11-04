import { Component, ElementRef, EventEmitter, inject, Input, Output, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { Placeholder } from '../../../core/enums/placehoder';
import { BrandingComponent } from '../sidebar/branding.component';
import { TooltipHeaderAction } from '../../../core/enums/tooltip';
import { TuiDataList, TuiDropdown, TuiIcon } from '@taiga-ui/core';
import { AlertService } from '../../../core/services/alert/alert.service';
import {TuiAvatar, TuiBadge, TuiBadgedContent, TuiBadgeNotification} from '@taiga-ui/kit';
import { UserDialogComponent } from '../../../shared/components/user-dialog/user-dialog.component';
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
    UserDialogComponent
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  @ViewChild('inputSearch') inputSearch: ElementRef;
  @Output() toggleSidebar = new EventEmitter<void>();
  placeholder = Placeholder;
  tooltipHeaderAction = TooltipHeaderAction;

  constructor(
    private alertService: AlertService,

  ) { }

  ngOnInit(
  ) { }

  focusSearchInput() {
    this.alertService.showSuccess('Search input focused', 'Search input focused');
    this.alertService.showError('Search input focused', 'Search input focused');
    this.alertService.showWarning('Search input focused', 'Search input focused');
    this.inputSearch.nativeElement.focus();
  }


  onToggleSidebar() {
    this.toggleSidebar.emit();
  }

}
