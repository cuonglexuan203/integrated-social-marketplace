import { Component, inject, INJECTOR, Input } from '@angular/core';
import { TuiButton, TuiDialogService, TuiDropdown, TuiIcon, TuiRoot } from '@taiga-ui/core';
import { TuiAvatar, TuiCarousel } from '@taiga-ui/kit';
import { TuiCardLarge } from '@taiga-ui/layout';
import { MockDataPost } from '../../../shared/mocks/mock-data-post';
import { CommonModule } from '@angular/common';
import { MoreDialogComponent } from '../../../shared/components/more-dialog/more-dialog.component';
import { ShareDialogComponent } from '../../../shared/components/share-dialog/share-dialog.component';
import {PolymorpheusComponent} from '@taiga-ui/polymorpheus';
import { ReactionDialogComponent } from '../../../shared/components/reaction-dialog/reaction-dialog.component';
import { FeedService } from '../../../core/services/feed/feed.service';
import { AlertService } from '../../../core/services/alert/alert.service';
import { FeedPost } from '../../../core/models/feed/feed.model';

@Component({
  selector: 'app-post-item',
  standalone: true,
  imports: [
    TuiAvatar,
    CommonModule,
    TuiIcon,
    TuiCarousel,
    TuiButton,
    MoreDialogComponent,
    TuiDropdown,
    ShareDialogComponent,
    ReactionDialogComponent
  ],
  templateUrl: './post-item.component.html',
  styleUrl: './post-item.component.css'
})
export class PostItemComponent {
  private readonly injector = inject(INJECTOR);
  private readonly dialogs = inject(TuiDialogService);

  @Input() post: FeedPost;
  currentIndex: number = 0;

  constructor(
    private _feedService: FeedService,
    private alertService: AlertService,
  ) {}

  ngOnInit() {
  }

  onCarouselChange(index: number) {
    this.currentIndex = index;
  }

  onShare() {
    const data = { title: 'Share', listSelection: ['On my wall', 'In a private message'] };
  
    this.dialogs
      .open(
        new PolymorpheusComponent(ShareDialogComponent, this.injector),
        {
          data: data,
          dismissible: false,
        }
      )
      .subscribe((data) => {
        console.log(data);
      });
  }

  
  
  
}

