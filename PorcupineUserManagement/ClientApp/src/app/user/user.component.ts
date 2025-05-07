import {Component, inject, OnInit} from '@angular/core';
import {AppService} from "../app.service";
import {HttpErrorResponse} from "@angular/common/module.d-CnjH8Dlt";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {UserDialogComponent} from "./user-dialog.component";
import {UserGroupDialogComponent} from "./user-group-dialog.component";
import {UserPermissionDialogComponent} from "./user-permission-dialog.component";

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrl: './user.component.css',
  standalone: false
})
export class UserComponent implements OnInit{
  data: any[] = [];
  totalCount: number = 0;
  error: any;
  displayedColumns: string[] = ['name', 'email', 'actions'];
  selectedRow: any = null;

  onRowClick(row: any) {
    this.selectedRow = row;
  }
  constructor(private appService: AppService) {
  }
  readonly dialog = inject(MatDialog);

  openDialog(component: any, enterAnimationDuration: string, exitAnimationDuration: string, data:any): MatDialogRef<any> {
    return this.dialog.open(component, {
      width: '80%',
      enterAnimationDuration,
      exitAnimationDuration,
      data: data
    });
  }
  ngOnInit(): void {
    this.getUsers();
  }
  getUsers(page: number = 1, pageSize: number = 10) {
    this.appService.getEntities('user', page, pageSize).subscribe(
      {
        next: (data) => {
          if (data)
          {
            this.data = data.data;
            this.totalCount = data.total;
          }
          else {
            this.data = [];
          }

        },
        error: (err: HttpErrorResponse) => {
          this.data = [];
          this.error = err;
        }
      }
    )
  }

  addUser() {
    let dialogRef = this.openDialog(UserDialogComponent,'10ms', '10ms',null);
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        if (result) {
          this.appService.createOrUpdateEntity('user', result).subscribe(
            {
              next: (data) => {
                this.getUsers();
              },
              error: (err: HttpErrorResponse) => {
                this.error = err;
              }
            }
          )
        }
      }
    });
  }

  onPageChange($event: any) {
    this.getUsers($event.pageIndex + 1, $event.pageSize);
  }

  editUser(row: any) {
    let dialogRef = this.openDialog( UserDialogComponent,'10ms', '10ms', row)
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        if (result) {
          this.appService.createOrUpdateEntity('user', result).subscribe(
            {
              next: (data) => {
                this.getUsers();
              },
              error: (err: HttpErrorResponse) => {
                this.error = err;
              }
            }
          )
        }
      }
    });
  }

  removeUser(row: any) {
    if (confirm('Are you sure you want to delete this user?')) {
      this.appService.deleteEntity('user', row.id).subscribe(
        {
          next: (data) => {
            this.getUsers();
          },
          error: (err: HttpErrorResponse) => {
            this.error = err;
          }
        }
      )
    }
  }
  userGroups(row:any) {
    this.openDialog(UserGroupDialogComponent,'10ms', '10ms', row)
  }
  userPermissions(row: any) {
    this.openDialog(UserPermissionDialogComponent,'10ms', '10ms', row)
  }
}
