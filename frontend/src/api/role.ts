import { api } from './client'

export interface RoleInfo {
  roleName: string
  roleId: string
}

// Every route here is [Authorize(Roles = "SuperAdmin")] on the backend, not Admin - stricter
// than every other admin-ish endpoint in the app (see WebApi/Controllers/RoleController.cs).
export const getRoles = () => api.get<RoleInfo[]>('/api/Role/GetRole')

export const addRoleToUser = (userId: string, roleId: string) => api.post<null>('/api/Role/AddRoleToUser', { userId, roleId })

// DeleteRoleFromUser is HttpDelete but takes a body (same DTO as AddRoleToUser) - real backend
// signature, not a client-side workaround; api.del's optional body param exists for this.
export const deleteRoleFromUser = (userId: string, roleId: string) => api.del<null>('/api/Role/DeleteRoleFromUser', { userId, roleId })
