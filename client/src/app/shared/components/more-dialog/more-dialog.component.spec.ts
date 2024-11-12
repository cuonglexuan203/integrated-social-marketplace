import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MoreDialogComponent } from './more-dialog.component';

describe('MoreDialogComponent', () => {
  let component: MoreDialogComponent;
  let fixture: ComponentFixture<MoreDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MoreDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MoreDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
