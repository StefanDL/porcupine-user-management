import {Component, Inject} from '@angular/core';
import {FormBuilder, FormGroup, Validators} from "@angular/forms";
import {MAT_DIALOG_DATA, MatDialogRef} from "@angular/material/dialog";
import {AppService} from "../app.service";

@Component({
  selector: 'app-group-user-add-dialog',
  standalone: false,
  template: `
    <h2 mat-dialog-title>Add User to Group</h2>

    <mat-dialog-content [formGroup]="form" class="m-3">
      <mat-form-field>
        <mat-label>User</mat-label>
        <mat-select formControlName="user">
          @for (user of users; track user) {
            <mat-option  [value]="user.id">{{user.name}}</mat-option>
          }
        </mat-select>
      </mat-form-field>
    </mat-dialog-content>

    <mat-dialog-actions align="end">
      <button mat-button mat-dialog-close>Cancel</button>
      <button mat-flat-button color="primary" (click)="save()">Save</button>
    </mat-dialog-actions>
  `,
  styles: ``
})
export class GroupUserAddDialogComponent {
  users: any[] = [];
  form: FormGroup;

  constructor(
    @Inject(MAT_DIALOG_DATA) public dataList: any[],
    private fb: FormBuilder,
    private appService : AppService,
    public dialogRef: MatDialogRef<GroupUserAddDialogComponent>
  ) {
    this.form = this.fb.group({
      user: ['', Validators.required]
    });
    this.appService.getEntityCount('user').subscribe(
      {
        next: (data: number) => {
          if (data)
          {
            this.appService.getEntities('user',1,data).subscribe(
            {
              next: (data: any) => {
                if (data)
                {
                  this.users = data.data.filter((x: { id: any; }) => !this.dataList.some(y => y.id === x.id));
                }
                else {
                  this.users = [];
                }
              }
            });
          }
        }
      }
    );
  }

  save(): void {
    if (this.form.valid) {
      this.dialogRef.close(this.form.value); // send data back
    }
  }
}
