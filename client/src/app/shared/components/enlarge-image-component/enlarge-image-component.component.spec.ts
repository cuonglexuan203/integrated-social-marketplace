import { ComponentFixture, TestBed } from '@angular/core/testing';

import { EnlargeImageComponentComponent } from './enlarge-image-component.component';

describe('EnlargeImageComponentComponent', () => {
  let component: EnlargeImageComponentComponent;
  let fixture: ComponentFixture<EnlargeImageComponentComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [EnlargeImageComponentComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(EnlargeImageComponentComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
