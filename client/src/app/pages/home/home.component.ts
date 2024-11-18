import { Component, Output } from '@angular/core';
import { PostItemComponent } from "./post-item/post-item.component";
import { MockDataPost } from '../../shared/mocks/mock-data-post';
import { FilterComponent } from '../../shared/components/filter/filter.component';
import { FilterData } from '../../core/data/filter-data';
import { Filter } from '../../core/models/filter/filter.model';
import { TagItemComponent } from './tag-item/tag-item.component';
import { AlertService } from '../../core/services/alert/alert.service';
import { FeedService } from '../../core/services/feed/feed.service';
import { FeedPost } from '../../core/models/feed/feed.model';
@Component({
  selector: 'app-home',
  standalone: true,
  imports: [
    PostItemComponent,
    TagItemComponent,
  ],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {
  @Output() posts: FeedPost[] = [];
  constructor(
    private _feedService: FeedService,
    private alertService: AlertService,
  ) { }

  ngOnInit() {
    this.getAllPosts();

  }

  getAllPosts() {
    this._feedService.getAllPosts().subscribe({
      next: (res) => {
        if (res) {
          this.posts = res.result;
        }
      },
      error: (err) => {
        this.alertService.showError('Get All Posts Fail' ,err.error.message);
      },
      complete: () => {
        
      }
    });
  }



}
