import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CommentPostDialogComponent } from './comment-post-dialog.component';

describe('CommentPostDialogComponent', () => {
  let component: CommentPostDialogComponent;
  let fixture: ComponentFixture<CommentPostDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CommentPostDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CommentPostDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
