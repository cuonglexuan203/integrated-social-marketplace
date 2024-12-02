import { Component } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { TuiButton, TuiDialogContext, TuiIcon } from '@taiga-ui/core';
import { TuiRadioList } from '@taiga-ui/kit';
import { injectContext } from '@taiga-ui/polymorpheus';
import { FeedPost } from '../../../core/models/feed/feed.model';
import { FeedService } from '../../../core/services/feed/feed.service';

@Component({
  selector: 'app-share-dialog',
  standalone: true,
  imports: [
    TuiRadioList,
    FormsModule,
    ReactiveFormsModule,
    TuiButton,
    TuiIcon,
  ],
  templateUrl: './share-dialog.component.html',
  styleUrls: ['./share-dialog.component.css']
})
export class ShareDialogComponent {
  public readonly context = injectContext<TuiDialogContext<any>>();
  data: any;
  title: string = '';
  listSelection: string[] = [];
  selectedItem: string | null = null; // Variable to track selected radio item
  post: FeedPost;
  newPost: string = '';
  constructor(
    private _feedService: FeedService,

  ) {
    this.data = this.context.data;
  }



  ngOnInit() {
    this.title = this.data.title;
    this.listSelection = this.data.listSelection;
    this.selectedItem = this.listSelection[0];
    this.post = this.data.post;
  }

  onSelectionChange() {
    if (this.selectedItem) {
      if (this.selectedItem === 'On my wall') {
        this.sharePost();
      }
    }
  }

  sharePost() {

  }
}
