const path = require('path');
const MiniCssExtractPlugin = require("mini-css-extract-plugin");
const { CleanWebpackPlugin } = require('clean-webpack-plugin');
const TerserPlugin = require('terser-webpack-plugin')
const CssMinimizerWebpackPlugin = require('css-minimizer-webpack-plugin')

const bundleFileName = 'bundle';
const dirName = 'wwwroot';

module.exports = (env, argv) => {
    return {
        mode: argv.mode === "production" ? "production" : "development",
        devtool: argv.mode === "production" ? false : "inline-source-map",
        // Cannot use multiple entry points when output type is library
        entry: './ClientApp/Scripts/main.ts',
        output: {
            filename: 'scripts/[name].' + bundleFileName + '.js',
            path: path.resolve(__dirname, dirName),
            // output type must be library in order to call functions from HTML
            library: 'AppLib'
        },
        resolve: {
            extensions: ['.ts', '.js', '.json']
        },
        module: {
            rules: [
                {
                    // Styles
                    test: /\.s?[ac]ss$/,
                    use: [
                        MiniCssExtractPlugin.loader,
                        {
                            loader: 'css-loader',
                            options: {
                                sourceMap: argv.mode !== "production"
                            }
                        },
                        {
                            loader: 'sass-loader',
                            options: {
                                sourceMap: argv.mode !== "production"
                            }
                        },
                    ]
                },
                {
                    // Scripts
                    test: /\.tsx?$/,
                    loader: 'ts-loader',
                    exclude: /node_modules/
                },
                {
                    // Fonts
                    test: /\.(woff|woff2|eot|ttf|otf)$/i,
                    type: "asset/resource",
                    generator: {
                        filename: "fonts/[name]-[hash].[ext]",
                    }
                }
            ]
        },
        optimization: {
            splitChunks: {
                cacheGroups: {
                    vendor: {
                        test: /[\\/]node_modules[\\/]/,
                        name: "vendor",
                        chunks: "all"
                    }
                }
            },
            minimize: argv.mode === "production",
            minimizer: [
                new TerserPlugin({
                    terserOptions: {
                        format: {
                            // do not emit license txt file
                            comments: argv.mode !== "production",
                        },
                    },
                    extractComments: false,
                }),
                new CssMinimizerWebpackPlugin()
            ],
        },
        plugins: [
            new CleanWebpackPlugin(),
            new MiniCssExtractPlugin({
                filename: 'styles/[name].' + bundleFileName + '.css'
            })
        ]
    };
};