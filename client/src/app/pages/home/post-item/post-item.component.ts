import { Component, inject, INJECTOR, Input } from '@angular/core';
import { TuiButton, TuiDialogService, TuiDropdown, TuiIcon, TuiRoot } from '@taiga-ui/core';
import { TuiAvatar, TuiCarousel, TuiPagination, TuiSkeleton } from '@taiga-ui/kit';
import { TuiCardLarge } from '@taiga-ui/layout';
import { MockDataPost } from '../../../shared/mocks/mock-data-post';
import { CommonModule } from '@angular/common';
import { MoreDialogComponent } from '../../../shared/components/more-dialog/more-dialog.component';
import { ShareDialogComponent } from '../../../shared/components/share-dialog/share-dialog.component';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { ReactionDialogComponent } from '../../../shared/components/reaction-dialog/reaction-dialog.component';
import { FeedService } from '../../../core/services/feed/feed.service';
import { AlertService } from '../../../core/services/alert/alert.service';
import { FeedPost } from '../../../core/models/feed/feed.model';
import { DateFilterPipe } from '../../../core/pipes/date-filter/date-filter.pipe';
import { Comment } from '../../../core/models/comment/comment.model';
import { CommentPostDialogComponent } from '../../../shared/components/comment-post-dialog/comment-post-dialog.component';
import {  ReactionModel, ReactionRequestModel } from '../../../core/models/reaction/reaction.model';
import { Helper } from '../../../core/utils/helper';
import { reactions } from '../../../core/constances/reaction';

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
    ReactionDialogComponent,
    TuiSkeleton,
    DateFilterPipe,
    TuiPagination
  ],
  templateUrl: './post-item.component.html',
  styleUrl: './post-item.component.css'
})
export class PostItemComponent {
  private readonly injector = inject(INJECTOR);
  private readonly dialogs = inject(TuiDialogService);
  @Input() isLoading: boolean;
  @Input() post: FeedPost;
  @Input() reactionReacted: any;
  currentIndex: number = 0;

  comments: Comment[] = [];
  reactions: ReactionModel[] = [];

  isReacted: boolean = false;
  reactionType: any;
  reactionsType = reactions;

  constructor(
    private _feedService: FeedService,
    private alertService: AlertService,

  ) { }

  ngOnInit() {
    this.onCheckedReaction();
    
  }

  getReactionReacted(event: any){
    if(event) {
      const dataForReact: ReactionRequestModel = {
        postId: this.post.id,
        userId: Helper.getUserFromLocalStorage().id,
        reactionType: event
      } 
      this._feedService.addReaction(dataForReact).subscribe({
        next: (res) => {
          if (res.result) {
            this.post.reactions.push(res.result);
            this.onCheckedReaction();
          }
        },
        error: (err) => {
          console.log(err);
        }
      });
    }
  }

  onCheckedReaction() {
    this.isLoading = true;
    this._feedService.getReactionsByPostId(this.post.id).subscribe({
      next: async (res) => {
        if (res.result) {
          await this.checkingUserHasReacted(res.result); 
          this.isLoading = false;
        }
        else {
          this.isLoading = false;
        }
      },
      error: (err) => {
        console.log(err);
        this.isLoading = false;
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  async checkingUserHasReacted(reactions: ReactionModel[]) {
    const user = Helper.getUserFromLocalStorage();
    const reaction = reactions.find((reaction) => reaction.user.id === user.id);
    this.isReacted = reaction ? true : false;
    this.reactionType = reaction ? reaction.type : '';
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

  
  onComment(post: FeedPost) {
    const data = { title: 'Comment', post: post };
    this.dialogs
      .open(
        new PolymorpheusComponent(CommentPostDialogComponent, this.injector),
        {
          dismissible: false,
          size: 'auto',
          data: data,
        }
      )
      .subscribe((data) => {
        console.log(data);
      });
  }


}

