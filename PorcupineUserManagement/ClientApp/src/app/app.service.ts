import {Inject, Injectable} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

@Injectable({
  providedIn: 'root'
})
export class AppService {
  private readonly baseUrl: string;

  constructor(@Inject('BASE_URL') baseUrl: string,private http: HttpClient) {
    this.baseUrl = baseUrl;
  }

  getEntityById(entityType: string, id: string): Observable<any> {
    return this.http.get(`${this.baseUrl}api/${entityType}/${id}`);
  }

  getEntities(entityType: string, page: number = 1, pageSize: number = 10 ): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}api/${entityType}`, {params: {page, pageSize}});
  }

  getEntityCount(entityType: string): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}api/${entityType}/count`);
  }

  createOrUpdateEntity(entityType: string, entity: any): Observable<any> {

    if (entity.id === null)
    {
      delete entity.id;
    }
    return this.http.post(`${this.baseUrl}api/${entityType}`, entity);
  }

  deleteEntity(entityType: string, id: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}api/${entityType}/${id}`);
  }

  getGroupUsers(groupId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}api/group/${groupId}/users`);
  }

  getGroupUsersCount(groupId: string): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}api/group/${groupId}/users/count`);
  }

  getGroupPermissions(groupId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}api/group/${groupId}/permissions`);
  }

  getGroupPermissionsCount(groupId: string): Observable<number> {
    return this.http.get<number>(`${this.baseUrl}api/group/${groupId}/permissions/count`);
  }

  addUserToGroup(groupId: string, userId: string): Observable<any> {
    return this.http.post(`${this.baseUrl}api/group/${groupId}/add-user/${userId}`, {});
  }

  removeUserFromGroup(groupId: string, userId: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}api/group/${groupId}/remove-user/${userId}`, {});
  }

  addPermissionToGroup(groupId: string, permissionId: string): Observable<any> {
    return this.http.post(`${this.baseUrl}api/group/${groupId}/add-permission/${permissionId}`, {});
  }

  removePermissionFromGroup(groupId: string, permissionId: string): Observable<any> {
    return this.http.delete(`${this.baseUrl}api/group/${groupId}/remove-permission/${permissionId}`, {});
  }

  getUserGroups(userId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}api/user/${userId}/groups`);
  }

  getUserPermissions(userId: string): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}api/user/${userId}/permissions`);
  }

}
