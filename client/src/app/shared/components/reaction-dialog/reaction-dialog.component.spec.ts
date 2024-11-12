import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ReactionDialogComponent } from './reaction-dialog.component';

describe('ReactionDialogComponent', () => {
  let component: ReactionDialogComponent;
  let fixture: ComponentFixture<ReactionDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ReactionDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ReactionDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
