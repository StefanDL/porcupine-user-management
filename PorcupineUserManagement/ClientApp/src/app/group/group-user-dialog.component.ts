import {Component, inject, Inject} from '@angular/core';
import {MAT_DIALOG_DATA, MatDialog, MatDialogRef} from "@angular/material/dialog";
import {FormBuilder} from "@angular/forms";
import {AppService} from "../app.service";
import {GroupUserAddDialogComponent} from "./group-user-add-dialog.component";
import {HttpErrorResponse} from "@angular/common/module.d-CnjH8Dlt";

@Component({
  selector: 'app-group-user-dialog',
  standalone: false,
  template: `
    <h2 mat-dialog-title>Group Users</h2>

    <mat-dialog-content class="m-3">
      <mat-table [dataSource]="dataList" class="w-100">
        <ng-container matColumnDef="name">
          <mat-header-cell *matHeaderCellDef> Name </mat-header-cell>
          <mat-cell *matCellDef="let row"> {{ row.name }} </mat-cell>
        </ng-container>
        <ng-container  matColumnDef="actions" >
          <mat-header-cell *matHeaderCellDef> Actions </mat-header-cell>
          <mat-cell *matCellDef="let row">
            <button matTooltip="Remove User" mat-icon-button (click)="removeUser(row)">
              <mat-icon>delete</mat-icon>
            </button>
          </mat-cell>

        </ng-container>
        <mat-header-row *matHeaderRowDef="displayedColumns"></mat-header-row>
        <mat-row mat-ripple *matRowDef="let row; columns: displayedColumns;">

        </mat-row>
      </mat-table>

    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Close</button>
      <button class="float-end" mat-fab color="primary" (click)="addUser()">
        <mat-icon class="mat-icon">add</mat-icon>
      </button>

    </mat-dialog-actions>
  `,
  styles: ``
})
export class GroupUserDialogComponent {
  displayedColumns: string[] = ['name', 'actions'];
  dataList: any[] = [];
  readonly dialog = inject(MatDialog);
  private error: any;

  openDialog(component: any, enterAnimationDuration: string, exitAnimationDuration: string, data:any): MatDialogRef<any> {
    return this.dialog.open(component, {
      width: '500px',
      enterAnimationDuration,
      exitAnimationDuration,
      data: data
    });

  }
  getGroupUsers()
  {
    this.appService.getGroupUsers(this.data.id).subscribe(
      {
        next: (data) => {
          if (data)
          {
            this.dataList = data;
          }
          else {
            this.dataList = [];
          }
        }
      }
    )
  }
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private appService: AppService,
    public dialogRef: MatDialogRef<GroupUserDialogComponent>
  ) {
    if (data) {
      this.getGroupUsers();
    }
  }

  addUser() {
    let dialogRef = this.openDialog(GroupUserAddDialogComponent,"10ms","10ms",this.dataList);
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        if (result) {
          this.appService.addUserToGroup(this.data.id,result.user).subscribe(
            {
              next: (data) => {
                this.getGroupUsers();
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
    if (confirm("Are you sure you want to remove this user from the group"))
    {
      this.appService.removeUserFromGroup(this.data.id,row.id).subscribe(
        {
          next: (data) => {
            this.getGroupUsers();
          },
          error: (err: HttpErrorResponse) => {
            this.error = err;
          }
        }
      )
    }
  }
}
