import { Component, ElementRef, EventEmitter, inject, Input, Output, ViewChild } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgScrollbarModule } from 'ngx-scrollbar';
import { Placeholder } from '../../../core/enums/placehoder';
import { BrandingComponent } from '../sidebar/branding.component';
import { TooltipHeaderAction } from '../../../core/enums/tooltip';
import { TuiDataList, TuiDropdown, TuiIcon, TuiRoot } from '@taiga-ui/core';
import { AlertService } from '../../../core/services/alert/alert.service';
import { TuiAvatar, TuiBadge, TuiBadgedContent, TuiBadgeNotification } from '@taiga-ui/kit';
import { UserDialogComponent } from '../../../shared/components/user-dialog/user-dialog.component';
import { WA_LOCAL_STORAGE, WA_WINDOW } from '@ng-web-apis/common';
import { TUI_DARK_MODE, TUI_DARK_MODE_KEY, TuiButton, TuiOption } from '@taiga-ui/core';

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
    TuiRoot,
  ],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
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


  setDarkMode() {
    this.darkMode.set(!this.darkMode())
  }
}
