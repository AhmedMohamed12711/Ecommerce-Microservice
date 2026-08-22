import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CollapseModule } from 'ngx-bootstrap/collapse';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';

import { CoreRoutingModule } from './core-routing.module';
import { NavbarComponent } from './Components/navbar/navbar.component';
import { NotFoundComponent } from './Pages/not-found/not-found.component';
import { NotAuthenticatedComponent } from './Pages/not-authenticated/not-authenticated.component';
import { ServerErrorComponent } from './Pages/server-error/server-error.component';
import { HeaderComponent } from './Components/header/header.component';
import { ToastContainerComponent } from './Components/toast-container/toast-container.component';
import { BreadcrumbComponent, BreadcrumbService } from 'xng-breadcrumb';
import { NgxSpinnerModule } from 'ngx-spinner';

@NgModule({
  declarations: [
    NavbarComponent,
    NotFoundComponent,
    NotAuthenticatedComponent,
    ServerErrorComponent,
    HeaderComponent,
    ToastContainerComponent,
  ],
  imports: [
    CommonModule,
    CoreRoutingModule,
    CollapseModule.forRoot(),
    BsDropdownModule.forRoot(),
    BreadcrumbComponent,
    NgxSpinnerModule,
  ],
  exports: [
    NavbarComponent,
    NotFoundComponent,
    NotAuthenticatedComponent,
    ServerErrorComponent,
    HeaderComponent,
    ToastContainerComponent,
    NgxSpinnerModule,
  ],
  providers: [
    BreadcrumbService,
  ]
})
export class CoreModule { }
