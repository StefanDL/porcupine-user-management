import {Component, inject, OnInit} from '@angular/core';
import {AppService} from "../app.service";
import {MatDialog, MatDialogRef} from "@angular/material/dialog";
import {GroupDialogComponent} from "./group-dialog.component";
import {HttpErrorResponse} from "@angular/common/module.d-CnjH8Dlt";
import {GroupUserDialogComponent} from "./group-user-dialog.component";
import {GroupPermissionDialogComponent} from "./group-permission-dialog.component";

@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrl: './group.component.css',
  standalone: false
})
export class GroupComponent implements OnInit{
  data: any[] = [];
  totalCount: number = 0;
  error: any;
  displayedColumns: string[] = ['name','actions'];
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
    this.getGroups();
  }
  getGroups(page: number = 1, pageSize: number = 10) {
    this.appService.getEntities('group', page, pageSize).subscribe(
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

  addGroup() {
    let dialogRef = this.openDialog(GroupDialogComponent,'10ms', '10ms',null)
    dialogRef.afterClosed().subscribe(result => {
      if (result !== undefined) {
        if (result) {
          this.appService.createOrUpdateEntity('group', result).subscribe(
            {
              next: (data) => {
                this.getGroups();
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
    this.getGroups($event.pageIndex + 1, $event.pageSize);
  }

  editGroup(row: any) {
    this.openDialog(GroupDialogComponent,'10ms', '10ms', row)
  }

  removeGroup(row: any) {
    if (confirm('Are you sure you want to delete this group?')) {
      this.appService.deleteEntity('group', row.id).subscribe(
        {
          next: (data) => {
            this.getGroups();
          },
          error: (err: HttpErrorResponse) => {
            this.error = err;
          }
        }
      )
    }
  }

  groupUsers(row: any) {
    this.openDialog(GroupUserDialogComponent,'10ms', '10ms', row)
  }

  groupPermissions(row: any) {
    this.openDialog(GroupPermissionDialogComponent,'10ms', '10ms', row)
  }
}

