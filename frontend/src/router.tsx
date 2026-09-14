import { createBrowserRouter } from 'react-router-dom'
import { Layout } from './components/Layout'
import { ProtectedRoute } from './auth/ProtectedRoute'
import { HomePage } from './pages/HomePage'
import { CatalogPage } from './pages/CatalogPage'
import { CategoryPage } from './pages/CategoryPage'
import { ProductListPage } from './pages/ProductListPage'
import { ProductDetailPage } from './pages/ProductDetailPage'
import { LoginPage } from './pages/LoginPage'
import { RegisterPage } from './pages/RegisterPage'
import { ProfilePage } from './pages/ProfilePage'
import { AddressesPage } from './pages/AddressesPage'
import { DeliveryAddressesPage } from './pages/DeliveryAddressesPage'
import { CartPage } from './pages/CartPage'
import { OrdersPage } from './pages/OrdersPage'
import { OrderDetailPage } from './pages/OrderDetailPage'
import { MyListingsPage } from './pages/MyListingsPage'
import { AddListingPage } from './pages/AddListingPage'
import { EditListingPage } from './pages/EditListingPage'
import { AdminCatalogPage } from './pages/AdminCatalogPage'
import { AdminRolesPage } from './pages/AdminRolesPage'
import { NotFoundPage } from './pages/NotFoundPage'

const BUSINESSMAN_ROLES = ['Businessman', 'Admin', 'SuperAdmin']
// Matches WebApi/Controllers/CatalogController.cs & friends: [Authorize(Roles = "SuperAdmin, Admin")].
const CATALOG_ADMIN_ROLES = ['Admin', 'SuperAdmin']
// RoleController is stricter - [Authorize(Roles = "SuperAdmin")] only, no Admin.
const ROLE_ADMIN_ROLES = ['SuperAdmin']

export const router = createBrowserRouter([
  {
    path: '/',
    element: <Layout />,
    children: [
      { index: true, element: <HomePage /> },
      { path: 'catalog/:catalogId', element: <CatalogPage /> },
      { path: 'category/:categoryId', element: <CategoryPage /> },
      { path: 'products/:productType', element: <ProductListPage /> },
      { path: 'products/:productType/:id', element: <ProductDetailPage /> },
      { path: 'login', element: <LoginPage /> },
      { path: 'register', element: <RegisterPage /> },
      {
        path: 'profile',
        element: (
          <ProtectedRoute>
            <ProfilePage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'addresses',
        element: (
          <ProtectedRoute>
            <AddressesPage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'delivery-addresses',
        element: (
          <ProtectedRoute>
            <DeliveryAddressesPage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'cart',
        element: (
          <ProtectedRoute>
            <CartPage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'orders',
        element: (
          <ProtectedRoute>
            <OrdersPage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'orders/:id',
        element: (
          <ProtectedRoute>
            <OrderDetailPage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'my-listings',
        element: (
          <ProtectedRoute roles={BUSINESSMAN_ROLES}>
            <MyListingsPage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'my-listings/new',
        element: (
          <ProtectedRoute roles={BUSINESSMAN_ROLES}>
            <AddListingPage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'my-listings/new/:productType',
        element: (
          <ProtectedRoute roles={BUSINESSMAN_ROLES}>
            <AddListingPage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'my-listings/:productType/:id/edit',
        element: (
          <ProtectedRoute roles={BUSINESSMAN_ROLES}>
            <EditListingPage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'admin/catalog',
        element: (
          <ProtectedRoute roles={CATALOG_ADMIN_ROLES}>
            <AdminCatalogPage />
          </ProtectedRoute>
        ),
      },
      {
        path: 'admin/roles',
        element: (
          <ProtectedRoute roles={ROLE_ADMIN_ROLES}>
            <AdminRolesPage />
          </ProtectedRoute>
        ),
      },
      { path: '*', element: <NotFoundPage /> },
    ],
  },
])
