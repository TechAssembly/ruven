import debug from 'debug';

export const ruvenDebug = debug('ruven');

export const debugLobbies = ruvenDebug.extend('lobbies');
export const debugErrors = ruvenDebug.extend('errors');
export const debugTeam = ruvenDebug.extend('teams');
