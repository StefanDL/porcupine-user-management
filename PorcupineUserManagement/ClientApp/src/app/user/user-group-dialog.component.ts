import {Component, Inject} from '@angular/core';
import {FormBuilder} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {AppService} from "../app.service";

@Component({
  selector: 'app-user-group-dialog',
  template: `
    <h2 mat-dialog-title>User Groups</h2>

    <mat-dialog-content class="m-3">
      <mat-table [dataSource]="dataList" class="w-100">
        <ng-container matColumnDef="name">
          <mat-header-cell *matHeaderCellDef> Name </mat-header-cell>
          <mat-cell *matCellDef="let row"> {{ row.name }} </mat-cell>
        </ng-container>
        <mat-header-row *matHeaderRowDef="displayedColumns"></mat-header-row>
        <mat-row mat-ripple *matRowDef="let row; columns: displayedColumns;">

        </mat-row>
      </mat-table>

    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Close</button>
    </mat-dialog-actions>
  `,
  styles: ``,
  standalone: false
})
export class UserGroupDialogComponent {
  displayedColumns: string[] = ['name'];
  dataList: any[] = [];
  constructor(
    @Inject(MAT_DIALOG_DATA) public data: any,
    private fb: FormBuilder,
    private appService: AppService,
    public dialogRef: MatDialogRef<UserGroupDialogComponent>
  ) {
    if (data) {
      this.appService.getUserGroups(data.id).subscribe(
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
  }
}
