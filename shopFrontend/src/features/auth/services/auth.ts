import { shopApi } from '../../common/api/shopApi';
import { AuthUserSchema } from '../../common/shemas/userSchema';
import type { AuthUser, AuthValues } from '../../common/types';
import type { GeneralResponse } from '../../pages/categories/services/categoryService';

export const login = async (data: AuthValues): Promise<AuthUser> => {
  const response = await shopApi.post('/auth/login', data);
  return AuthUserSchema.parse(response);
}

interface RefreshTokenDTO {
  token: string
  refreshToken: string
}
export const refreshToken = async (data: RefreshTokenDTO): Promise<AuthUser> => {
  const res = (await shopApi.post('/auth/refreshToken', data)) as AuthUser
  return AuthUserSchema.parse(res)
}

interface RegisterDTO {
  userName: string;
  name: string;
  lastName: string;
  email: string;
}

export const register = async (data: RegisterDTO) => {
  console.log('Register data:', data);
  return await shopApi.post('/auth/register', data) as GeneralResponse;
}