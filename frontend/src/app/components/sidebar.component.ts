import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BrandComponent } from './brand.component';
import { SidebarSectionComponent } from './sidebar-section.component';
import { SidebarNavItemComponent } from './sidebar-nav-item.component';

@Component({
  selector: 'app-sidebar',
  standalone: true,
  imports: [
    CommonModule,
    BrandComponent,
    SidebarSectionComponent,
    SidebarNavItemComponent
  ],
  template: `
    <aside class="w-64 h-full bg-[#0d0d0f] border-r border-zinc-800/80 flex flex-col justify-between select-none">
      <!-- Top Branding and Navigation -->
      <div class="flex flex-col">
        <app-brand></app-brand>

        <div class="mt-5 flex flex-col gap-1">
          <!-- MAIN Section -->
          <app-sidebar-section title="Main">
            <app-sidebar-nav-item
              label="Dashboard"
              [isActive]="true"
              icon="dashboard"
            ></app-sidebar-nav-item>
          </app-sidebar-section>

          <!-- DIET Section -->
          <app-sidebar-section title="Diet">
            <!-- Diet Dropdown Header -->
            <app-sidebar-nav-item
              label="Diet"
              [isActive]="false"
              icon="diet"
              [hasChevron]="true"
              class="mb-1"
            ></app-sidebar-nav-item>

            <!-- Indented Sub-menu Items -->
            <div class="pl-4 flex flex-col gap-0.5 border-l border-zinc-800/30 ml-5.5">
              <app-sidebar-nav-item
                label="Ingredients"
                [isActive]="false"
                icon="ingredients"
              ></app-sidebar-nav-item>
              <app-sidebar-nav-item
                label="Meal Items"
                [isActive]="false"
                icon="meal-items"
              ></app-sidebar-nav-item>
              <app-sidebar-nav-item
                label="Meals"
                [isActive]="false"
                icon="meals"
              ></app-sidebar-nav-item>
              <app-sidebar-nav-item
                label="Meals Per Day"
                [isActive]="false"
                icon="meals-per-day"
              ></app-sidebar-nav-item>
              <app-sidebar-nav-item
                label="Units of Measure"
                [isActive]="false"
                icon="units"
              ></app-sidebar-nav-item>
            </div>
          </app-sidebar-section>
        </div>
      </div>
    </aside>
  `
})
export class SidebarComponent {}
