import { Component, inject, INJECTOR, Input, Output, SimpleChanges } from '@angular/core';
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
import { ReactionModel, ReactionRequestModel } from '../../../core/models/reaction/reaction.model';
import { Helper } from '../../../core/utils/helper';
import { reactions } from '../../../core/constances/reaction';
import { FirstLetterWordPipe } from '../../../core/pipes/first-letter-word/first-letter-word.pipe';
import { CommentService } from '../../../core/services/comment/comment.service';
import { TuiTagModule } from '@taiga-ui/legacy';
import { UserPostDialogComponent } from '../../../shared/components/user-post-dialog/user-post-dialog.component';
import { ClipboardModule } from '@angular/cdk/clipboard';
import { CommentStateService } from '../../../core/services/state/comment-state/comment-state.service';


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
    TuiPagination,
    TuiSkeleton,
    FirstLetterWordPipe,
    TuiTagModule,
    UserPostDialogComponent,
    ClipboardModule
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
  @Output() sharePost: FeedPost
  currentIndex: number = 0;

  comments: Comment[] = [];
  reactions: ReactionModel[] = [];

  isReacted: boolean = false;
  reactionType: any;
  reactionsType = reactions;

  dropdownHideDelay = 200;
  isInitial = false;
  constructor(
    private _feedService: FeedService,
    private alertService: AlertService,
    private _commentService: CommentService,
    private _commentStateService: CommentStateService
  ) { }

  ngOnInit() {
    this.initializePostState();
  }


  initializePostState() {
    this.isInitial = true;
    // Reset state for the new post
    this.comments = [];
    this.isReacted = false;
    this.reactionType = '';
    this.currentIndex = 0;

    if (this.post) {
      this.getCommentsPost();
      this.onCheckedReaction();
    }
  }





  getCommentsPost() {
    this.isLoading = true;
    this._commentService.getCommentsByPostId(this.post.id).subscribe({
      next: (res) => {
        if (res) {
          this.comments = res.result || [];
          this.isLoading = false;
        }
      },
      error: (error) => {
        console.log(error);
        this.isLoading = false;
      },
      complete: () => {
        this.isLoading = false;
      }
    });
  }

  onRemoveReaction() {
    const dataSending = {
      postId: this.post.id,
      userId: Helper.getUserFromLocalStorage().id,
    }
    this._feedService.removeReactionFromPost(dataSending).subscribe({
      next: (res) => {
        if (res.result) {
          this.isReacted = false;
          this.reactionType = '';
          this.post.reactions = this.post.reactions.filter((reaction) => reaction.user.id !== Helper.getUserFromLocalStorage().id);
        }
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
      }
    })
  }
  getReactionReacted(event: any) {
    console.log(event);

    if (this.isReacted && event === this.reactionType) {
      this.onRemoveReaction();
    }
    if (this.isReacted && event !== this.reactionType) {
      this.onRemoveReaction();
      this.onReactionWithNew(event);
    }
    if (!this.isReacted) {
      this.onReactionWithNew(event);
    }
  }

  onReactionWithNew(event: any) {
    if (event !== null && event !== undefined) {
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
    this._feedService.getReactionsByPostId(this.post.id).subscribe({
      next: async (res) => {
        if (res.result) {
          await this.checkingUserHasReacted(res.result);
        }
        else {
        }
      },
      error: (err) => {
        console.log(err);
      },
      complete: () => {
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
    const data = { title: 'Share', listSelection: ['On my wall', 'In a private message'], post: this.post };

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

  onCopy() {
    this.alertService.showSuccess('The link has been copied to your clipboard', 'Success');
  }

}

