import { Component, EventEmitter, Input, Output } from '@angular/core';
import { TuiHint } from '@taiga-ui/core';
import { TuiTooltip } from '@taiga-ui/kit';
import { FeedPost } from '../../../core/models/feed/feed.model';
import { UserResponseModel } from '../../../core/models/user/user.model';
import { Helper } from '../../../core/utils/helper';
import { FeedService } from '../../../core/services/feed/feed.service';
import { AlertService } from '../../../core/services/alert/alert.service';
import { ReactionRequestModel } from '../../../core/models/reaction/reaction.model';
import { reactions } from '../../../core/constances/reaction';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-reaction-dialog',
  standalone: true,
  imports: [
    TuiHint,
    TuiTooltip,
    CommonModule
  ],
  templateUrl: './reaction-dialog.component.html',
  styleUrl: './reaction-dialog.component.css'
})
export class ReactionDialogComponent {
  @Input() postItem: FeedPost;
  @Input() reactedReactionType: any;
  @Input() isReacted: boolean;
  @Output() onReaction: EventEmitter<any> = new EventEmitter<any>();
  user: UserResponseModel;
  reactions = reactions;
  constructor(
    private _feedService: FeedService,
    private alertService: AlertService
  ) {

  }

  onClickReaction(reactionType: number) {
    console.log(reactionType);
    
    if (reactionType !== null && reactionType !== undefined) {
      this.onReaction.emit(reactionType);
    }
  }

  ngOnInit() {
    this.user = Helper.getUserFromLocalStorage();
  }


}
