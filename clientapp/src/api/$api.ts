import type { AspidaClient, BasicHeaders } from 'aspida';
import type { Methods as Methods_1iqrdh0 } from './api/greeting';

const api = <T>({ baseURL, fetch }: AspidaClient<T>) => {
  const prefix = (baseURL === undefined ? '' : baseURL).replace(/\/$/, '');
  const PATH0 = '/api/greeting';
  const GET = 'GET';

  return {
    api: {
      greeting: {
        /**
         * @returns OK
         */
        get: (option?: { config?: T | undefined } | undefined) =>
          fetch<Methods_1iqrdh0['get']['resBody'], BasicHeaders, Methods_1iqrdh0['get']['status']>(prefix, PATH0, GET, option).json(),
        /**
         * @returns OK
         */
        $get: (option?: { config?: T | undefined } | undefined) =>
          fetch<Methods_1iqrdh0['get']['resBody'], BasicHeaders, Methods_1iqrdh0['get']['status']>(prefix, PATH0, GET, option).json().then(r => r.body),
        $path: () => `${prefix}${PATH0}`,
      },
    },
  };
};

export type ApiInstance = ReturnType<typeof api>;
export default api;
