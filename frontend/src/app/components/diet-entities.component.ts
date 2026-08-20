import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EntityCardComponent } from './entity-card.component';

@Component({
  selector: 'app-diet-entities',
  standalone: true,
  imports: [CommonModule, EntityCardComponent],
  template: `
    <section class="select-none">
      <!-- Section Title -->
      <h2 class="text-[0.6875rem] font-bold text-zinc-500 tracking-wider uppercase font-mono mb-4">
        Diet Entities
      </h2>

      <!-- Entities Grid -->
      <div class="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-4">
        @for (entity of entities(); track entity.title) {
          <app-entity-card
            [count]="entity.count"
            [title]="entity.title"
            [description]="entity.description"
            actionText="Open"
          ></app-entity-card>
        }
      </div>
    </section>
  `
})
export class DietEntitiesComponent {
  entities = input<Array<{ count: number | string; title: string; description: string }>>([
    { count: 8, title: 'Ingredients', description: 'Raw components' },
    { count: 4, title: 'Meal Items', description: 'Prepared foods' },
    { count: 3, title: 'Meals', description: 'Occasions' },
    { count: 3, title: 'Meals Per Day', description: 'Daily slots' },
    { count: 9, title: 'Units of Measure', description: 'Conversions' }
  ]);
}
