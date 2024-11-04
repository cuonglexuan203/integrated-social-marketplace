import { Component, Input } from '@angular/core';
import { TuiIcon, TuiSurface } from '@taiga-ui/core';
import { Filter } from '../../../core/models/filter/filter.model';

@Component({
  selector: 'app-filter',
  standalone: true,
  imports: [
    TuiSurface,
    TuiIcon 
  ],
  templateUrl: './filter.component.html',
  styleUrl: './filter.component.css'
})
export class FilterComponent {
  @Input() filterItem: Filter;
  constructor() { }
  ngOnInit() {}

}
