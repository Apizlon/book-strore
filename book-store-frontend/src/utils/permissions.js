export const hasPermission = (userPermissions, requiredPermission) => {
  return userPermissions && userPermissions.includes(requiredPermission);
};

export const hasAnyPermission = (userPermissions, permissions) => {
  return userPermissions && permissions.some(p => userPermissions.includes(p));
};

export const hasAllPermissions = (userPermissions, permissions) => {
  return userPermissions && permissions.every(p => userPermissions.includes(p));
};
