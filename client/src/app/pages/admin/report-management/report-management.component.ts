import { Component, inject, INJECTOR } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { AnimationOptions, LottieComponent } from 'ngx-lottie';
import { TuiDialogService } from '@taiga-ui/core';
import { ConfirmationDialogComponent } from '../../../shared/components/confirmation-dialog/confirmation-dialog.component';
import { PolymorpheusComponent } from '@taiga-ui/polymorpheus';
import { FeedService } from '../../../core/services/feed/feed.service';
import { Page } from '../../../core/models/page/page.model';
import { FeedPost } from '../../../core/models/feed/feed.model';
import { CommentPostDialogComponent } from '../../../shared/components/comment-post-dialog/comment-post-dialog.component';
import { TuiSkeleton } from '@taiga-ui/kit';
import { UpdateValidityModel } from '../../../core/models/feed/report.model';
import { AlertService } from '../../../core/services/alert/alert.service';


interface Report {
  id: number;
  title: string;
  date: string;
  status: 'Completed' | 'In Progress' | 'Pending';
}

@Component({
  selector: 'app-report-management',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    LottieComponent,
    TuiSkeleton,
  ],
  templateUrl: './report-management.component.html',
  styleUrl: './report-management.component.css'
})
export class ReportManagementComponent {

  private readonly injector = inject(INJECTOR);
  private readonly dialogs = inject(TuiDialogService);

  reports: any[] = [];

  options: AnimationOptions = {
    path: 'assets/animations/report.json',
    loop: true,
  };

  currentPage = 1;
  itemsPerPage = 12;

  page: Page = {
    pageIndex: 1,
    pageSize: 12,
    sort: 'desc',
  };
  isLoading = false;

  totalPages: number;
  totalItems: number;

  reportPage: any[] = []
  constructor(
    private _feedService: FeedService,
    private alertService: AlertService
  ) { }

  ngOnInit() {
    this.getAllReports();
    console.log(this.reports);

  }
  getAllReports() {
    this.isLoading = true;
    this._feedService.getAllReports(this.page).subscribe({
      next: (res) => {
        if (res) {
          this.reports = res?.result?.data;
          this.totalItems = res?.result?.count;
          this.totalPages = this.getTotalsPages(this.totalItems);
          this.isLoading = false;
        }
      },
      error: (err) => {
        console.error(err);
        this.isLoading = false;
      },
      complete: () => {
        console.log('complete');
        this.isLoading = false;
      }
    })
  }

  getTotalsPages(totalItems: number): number {
    return Math.ceil(totalItems / 10);
  }




  getStatusClass(validity: boolean): any {
    if (validity === null) {
      return 'UnReview'
    }
    else {
      if (validity === true) {
        return 'Valid'
      }
      else {
        return 'Invalid'
      }

    }
  }

  changePage(pageIndex: number): void {
    this.page = {
      pageIndex: pageIndex,
      pageSize: this.itemsPerPage,
      sort: 'desc',
    }
    this.getAllReports();

  }

  updateReportValidity(report: any) {
    const data = { title: 'Update Report', listSelection: ['Valid', 'Invalid'], reportValidity: report?.validity};
    this.dialogs
      .open(
        new PolymorpheusComponent(ConfirmationDialogComponent, this.injector),
        {
          data: data,
          dismissible: false,
        }
      )
      .subscribe((data) => {
        const sendingData: UpdateValidityModel = {
          reportId: report?.id,
          validity: data as any,
        }
        this.isLoading = true;
        this._feedService.updateReportValidity(sendingData).subscribe({
          next: (res) => {
            if (res) {
              this.getAllReports();
              this.isLoading = false;
            }
          },
          error: (err) => {
            console.error(err);
            this.alertService.showError('Update report validity fail', 'Error');
            this.isLoading = false;
          },
          complete: () => {
            this.alertService.showSuccess('Update report validity successfully', 'Success');
            this.isLoading = false;
          }
        });

      });
  }

  openPostDetail(post: FeedPost) {
    const data = { title: 'Review Post', post: post };
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
  }
}