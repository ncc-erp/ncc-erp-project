import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import {
  Router,
  RouterEvent,
  NavigationEnd,
  PRIMARY_OUTLET,
} from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { filter } from 'rxjs/operators';
import { MenuItem } from '@shared/layout/menu-item';

@Component({
  selector: 'sidebar-menu',
  templateUrl: './sidebar-menu.component.html',
  styleUrls: ['./sidebar-menu.component.css']
})
export class SidebarMenuComponent extends AppComponentBase implements OnInit {
  menuItems: MenuItem[];
  menuItemsMap: { [key: number]: MenuItem } = {};
  activatedMenuItems: MenuItem[] = [];
  routerEvents: BehaviorSubject<RouterEvent> = new BehaviorSubject(undefined);
  homeRoute = '/app/home';

  constructor(injector: Injector, private router: Router) {
    super(injector);
    this.router.events.subscribe(this.routerEvents);
  }

  ngOnInit(): void {
    this.menuItems = this.getMenuItems();
    this.patchMenuItems(this.menuItems);
    this.routerEvents
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe((event) => {
        const currentUrl = event.url !== '/' ? event.url : this.homeRoute;
        const primaryUrlSegmentGroup =
          this.router.parseUrl(currentUrl).root.children[PRIMARY_OUTLET];
        if (primaryUrlSegmentGroup) {
          this.activateMenuItems('/' + primaryUrlSegmentGroup.toString());
        }
      });
  }

  getMenuItems(): MenuItem[] {
    return [
      new MenuItem(
        this.l('Admin'),
        '',
        'fas fa-user-cog',
        'Admin',
        [
          new MenuItem(
            this.l('Tenants'),
            '/app/tenants',
            'fas fa-users-cog',
            'Admin.Tenants'
          ),
          new MenuItem(
            this.l('Clients'),
            '/app/clients',
            'fas fa-user-friends',
            'Admin.Clients'
          ),
          new MenuItem(
            this.l('Branches'),
            '/app/branchs',
            'fas fa-building',
            'Admin.Branchs'
          ),
          new MenuItem(
            this.l('Positions'),
            '/app/positions',
            'fas fa-anchor',
            'Admin.Positions'
          ),
          new MenuItem(
            this.l('Project Technologies'),
            '/app/technologies',
            'fas fa-info',
            'Admin.Technologies'
          ),
          new MenuItem(
            this.l('Configurations'),
            '/app/configurations',
            'fas fa-cog',
            'Admin.Configuartions'
          ),
          new MenuItem(
            this.l('Skills'),
            '/app/skills',
            'fas fa-cogs',
            'Admin.Skills'
          ),
          new MenuItem(
            this.l('Currencies'),
            '/app/currency',
            'fas fa-money-check',
            'Admin.Currencies'
          ),
          new MenuItem(
            this.l('Users'),
            '/app/users',
            'fas fa-users',
            'Admin.Users'
          ),
          new MenuItem(
            this.l('Roles'),
            '/app/roles',
            'fas fa-theater-masks',
            'Admin.Roles'
          ),
          new MenuItem(
            this.l('Audit Logs'),
            '/app/audit-logs',
            'fas fa-cogs',
            'Admin.AuditLogs.View'
          ),
          new MenuItem(
            this.l('Criteria '),
            '/app/criterias',
            'fas fa-tasks',
            'Admin.Criteria'
          )
        ]
      ),
      new MenuItem(
        this.l('Projects'),
        '',
        'fas fa-user-tie',
        'Projects',
        [
          new MenuItem(
            this.l('Outsourcing Projects'),
            '/app/list-project',
            'fas fa-project-diagram',
            'Projects.OutsourcingProjects'
          ),
          new MenuItem(
            this.l('Training Projects'),
            '/app/training-projects',
            'fas fa-chalkboard',
            'Projects.TrainingProjects'
          ),
          new MenuItem(
            this.l('Product Projects'),
            '/app/product-projects',
            'fab fa-product-hunt',
            'Projects.ProductProjects'
          ),
        ]
      ),

      // new MenuItem(
      //   this.l('Sao đỏ'),
      //   '/app/sao-do',
      //   'fas fa-user-shield',
      //   'SaoDo.CanViewMenu'
      // ),
      // new MenuItem(
      //   this.l('CheckList'),
      //   '',
      //   'fas fa-tasks',
      //   'CheckList.CanviewMenu',
      //   [
      //     new MenuItem(
      //       this.l('Checklist Category'),
      //       '/app/checklist-title',
      //       'fas fa-clipboard-list',
      //       'CheckList.CheckListCategory'
      //     ),
      //     new MenuItem(
      //       this.l('Checklist Item'),
      //       '/app/checklist',
      //       'fas fa-calendar-check',
      //       'CheckList.CheckListItem'
      //     ),
      //   ]
      // ),
      new MenuItem(
        this.l('Weekly Reports'),
        '/app/weekly-report',
        'fas fa-chalkboard-teacher',
        'WeeklyReport'
      ),
      new MenuItem(
        this.l('Resource Requests'),
        '/app/resource-request',
        'fab fa-chromecast',
        'ResourceRequest'
      ),
      new MenuItem(
        this.l('Training Requests'),
        '/app/training-request',
        'fas fa-mail-bulk',
        'TrainingRequest'
      ),
      new MenuItem(
        this.l('Resources'),
        '/app/available-resource/pool',
        'fas fa-hockey-puck',
        'Resource'
      ),
      new MenuItem(
        this.l('Timesheets'),
        '/app/timesheet',
        'fas fa-calendar-alt',
        'Timesheets'
      ),
      new MenuItem(
        this.l('Audits'),
        '',
        'fas fa-clipboard-check',
        'Audits',
        [
          new MenuItem(
            this.l('Criteria'),
            '/app/criteria-management',
            'fas fa-check-circle',
            'Audits.Criteria'
          ),
          new MenuItem(
            this.l('Tailoring'),
            '/app/tailoring-management',
            'fas fa-chalkboard',
            'Audits.Tailoring'
          ),
          new MenuItem(
            this.l('Results'),
            '/app/audit-result',
            'fas fa-thumbs-up',
            'Audits.Results'
          ),
        ]
      ),
    ];
  }

  patchMenuItems(items: MenuItem[], parentId?: number): void {
    items.forEach((item: MenuItem, index: number) => {
      item.id = parentId ? Number(parentId + '' + (index + 1)) : index + 1;
      if (parentId) {
        item.parentId = parentId;
      }
      if (parentId || item.children) {
        this.menuItemsMap[item.id] = item;
      }
      if (item.children) {
        this.patchMenuItems(item.children, item.id);
      }
    });
  }

  activateMenuItems(url: string): void {
    this.deactivateMenuItems(this.menuItems);
    this.activatedMenuItems = [];
    const foundedItems = this.findMenuItemsByUrl(url, this.menuItems);
    foundedItems.forEach((item) => {
      this.activateMenuItem(item);
    });
  }

  deactivateMenuItems(items: MenuItem[]): void {
    items.forEach((item: MenuItem) => {
      item.isActive = false;
      item.isCollapsed = true;
      if (item.children) {
        this.deactivateMenuItems(item.children);
      }
    });
  }

  findMenuItemsByUrl(
    url: string,
    items: MenuItem[],
    foundedItems: MenuItem[] = []
  ): MenuItem[] {
    items.forEach((item: MenuItem) => {
      if (item.route === url) {
        foundedItems.push(item);
      } else if (item.children) {
        this.findMenuItemsByUrl(url, item.children, foundedItems);
      }
    });
    return foundedItems;
  }

  activateMenuItem(item: MenuItem): void {
    item.isActive = true;
    if (item.children) {
      item.isCollapsed = false;
    }
    this.activatedMenuItems.push(item);
    if (item.parentId) {
      this.activateMenuItem(this.menuItemsMap[item.parentId]);
    }
  }

  isMenuItemVisible(item: MenuItem): boolean {
    if (!item.permissionName) {
      return true;
    }
    return this.permission.isGranted(item.permissionName);
  }
}
