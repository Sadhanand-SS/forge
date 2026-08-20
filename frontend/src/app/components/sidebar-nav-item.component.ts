import { Component, input } from '@angular/core';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-sidebar-nav-item',
  standalone: true,
  imports: [CommonModule],
  template: `
    <a
      href="javascript:void(0)"
      [class]="
        'flex items-center justify-between px-3 py-2.5 rounded-lg text-sm transition-all duration-150 group ' +
        (isActive()
          ? 'bg-[#ea580c] text-white font-medium shadow-md shadow-orange-600/10'
          : 'text-zinc-400 hover:bg-zinc-800/40 hover:text-zinc-200')
      "
    >
      <div class="flex items-center gap-3">
        <!-- Icons based on type -->
        <span class="flex items-center justify-center w-5 h-5">
          @if (icon() === 'dashboard') {
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="w-4.5 h-4.5">
              <rect x="3" y="3" width="7" height="7" />
              <rect x="14" y="3" width="7" height="7" />
              <rect x="14" y="14" width="7" height="7" />
              <rect x="3" y="14" width="7" height="7" />
            </svg>
          } @else if (icon() === 'ingredients') {
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" class="w-4.5 h-4.5">
              <circle cx="12" cy="12" r="8" />
            </svg>
          } @else if (icon() === 'meal-items') {
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="w-4.5 h-4.5">
              <path d="M12 2L2 7l10 5 10-5-10-5zM2 17l10 5 10-5M2 12l10 5 10-5" />
            </svg>
          } @else if (icon() === 'meals') {
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="w-4.5 h-4.5">
              <path d="M12 2v20M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6" />
            </svg>
          } @else if (icon() === 'meals-per-day') {
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="w-4.5 h-4.5">
              <circle cx="12" cy="12" r="10" />
              <polyline points="12 6 12 12 16 14" />
            </svg>
          } @else if (icon() === 'units') {
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="w-4.5 h-4.5">
              <path d="M20.59 13.41l-7.17 7.17a2 2 0 0 1-2.83 0L2 12V2h10l8.59 8.59a2 2 0 0 1 0 2.82z" />
              <line x1="7" y1="7" x2="7.01" y2="7" />
            </svg>
          } @else if (icon() === 'diet') {
            <svg xmlns="http://www.w3.org/2000/svg" viewBox="0 0 24 24" fill="none" stroke="currentColor" stroke-width="2" stroke-linecap="round" stroke-linejoin="round" class="w-4.5 h-4.5">
              <path d="M12 20h9M3 20v-8a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v8M3 12V6a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v6" />
            </svg>
          }
        </span>
        <span class="text-[0.875rem] font-medium tracking-wide">{{ label() }}</span>
      </div>

      <!-- Optional Chevron for dropdowns or expandable items -->
      @if (hasChevron()) {
        <svg
          xmlns="http://www.w3.org/2000/svg"
          viewBox="0 0 24 24"
          fill="none"
          stroke="currentColor"
          stroke-width="2"
          stroke-linecap="round"
          stroke-linejoin="round"
          class="w-3.5 h-3.5 text-zinc-500 group-hover:text-zinc-300 transition-colors"
        >
          <polyline points="6 9 12 15 18 9" />
        </svg>
      }
    </a>
  `
})
export class SidebarNavItemComponent {
  label = input.required<string>();
  isActive = input<boolean>(false);
  icon = input.required<string>();
  hasChevron = input<boolean>(false);
}
