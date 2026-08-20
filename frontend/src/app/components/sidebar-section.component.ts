import { Component, input } from '@angular/core';

@Component({
  selector: 'app-sidebar-section',
  standalone: true,
  template: `
    <div class="mb-5">
      <div class="px-6 py-2 text-[0.6875rem] font-bold text-zinc-500 tracking-wider uppercase font-mono mb-1.5">
        {{ title() }}
      </div>
      <div class="px-3 flex flex-col gap-1">
        <ng-content></ng-content>
      </div>
    </div>
  `
})
export class SidebarSectionComponent {
  title = input.required<string>();
}
