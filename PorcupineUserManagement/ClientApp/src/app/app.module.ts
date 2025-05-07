import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import {FormsModule, ReactiveFormsModule} from '@angular/forms';
import {provideHttpClient, withInterceptorsFromDi} from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import {User} from "oidc-client";
import {UserComponent} from "./user/user.component";
import {GroupComponent} from "./group/group.component";
import {PermissionComponent} from "./permission/permission.component";
import {MatButton, MatFabButton, MatIconButton} from "@angular/material/button";
import {MatCard, MatCardActions, MatCardContent, MatCardTitle} from "@angular/material/card";
import {MatIcon} from "@angular/material/icon";
import {MatTable, MatTableModule} from "@angular/material/table";
import {MatPaginatorModule} from "@angular/material/paginator";
import {MatRippleModule} from "@angular/material/core";
import {MatDivider} from "@angular/material/divider";
import {UserDialogComponent} from "./user/user-dialog.component";
import {
  MatDialogModule,
} from "@angular/material/dialog";
import {MatFormFieldModule} from "@angular/material/form-field";
import {MatInputModule} from "@angular/material/input";
import {GroupDialogComponent} from "./group/group-dialog.component";
import {PermissionDialogComponent} from "./permission/permission-dialog.component";
import {MatTooltipModule} from "@angular/material/tooltip";
import {UserGroupDialogComponent} from "./user/user-group-dialog.component";
import {UserPermissionDialogComponent} from "./user/user-permission-dialog.component";
import {GroupUserDialogComponent} from "./group/group-user-dialog.component";
import {GroupPermissionDialogComponent} from "./group/group-permission-dialog.component";
import {GroupUserAddDialogComponent} from "./group/group-user-add-dialog.component";
import {MatSelectModule} from "@angular/material/select";
import {GroupPermissionAddDialogComponent} from "./group/group-permission-add-dialog.component";
import {MatGridList, MatGridTile} from "@angular/material/grid-list";
import {MatExpansionModule, MatExpansionPanel} from "@angular/material/expansion";

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    UserComponent,
    GroupComponent,
    PermissionComponent,
    UserDialogComponent,
    GroupDialogComponent,
    PermissionDialogComponent,
    UserGroupDialogComponent,
    UserPermissionDialogComponent,
    GroupUserDialogComponent,
    GroupPermissionDialogComponent,
    GroupUserAddDialogComponent,
    GroupPermissionAddDialogComponent
  ],
  imports: [
    FormsModule,
    BrowserModule,
    RouterModule.forRoot([
      {path: '', component: HomeComponent, pathMatch: 'full'},
      {path: 'users', component: UserComponent},
      {path: 'groups', component: GroupComponent},
      {path: 'permissions', component: PermissionComponent},
    ]),
    MatFabButton,
    MatCard,
    MatCardTitle,
    MatCardContent,
    MatCardActions,
    MatIcon,
    MatTableModule,
    MatPaginatorModule,
    MatRippleModule,
    MatDivider,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    MatButton,
    MatIconButton,
    MatTooltipModule,
    MatSelectModule,
    MatGridList,
    MatGridTile,
    MatExpansionModule
  ],
  providers: [
    provideHttpClient(withInterceptorsFromDi())
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
